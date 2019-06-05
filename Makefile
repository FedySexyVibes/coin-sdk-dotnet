include Makefile.mk

ALL=common-sdk number-portability-sdk number-portability-sdk-samples
ALL_DEPLOY=common-sdk number-portability-sdk

tag-all-patch-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-patch-release ; done

tag-all-minor-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-minor-release ; done

tag-all-major-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-major-release ; done

version-check-java:
	for DIR in $(JVM); do $(MAKE) -C $$DIR version-check-java ; done

qa:
	for DIR in $(JVM); do $(MAKE) -C $$DIR qa; done

dependency-vulnerability-check-java:
	for DIR in $(JVM); do $(MAKE) -C $$DIR dependency-vulnerability-check-java; done

package:
	mvn -Dgit_release=${VERSION} ${MVN_OPTIONS} -B package versions:display-dependency-updates

test:
	mvn -Dgit_release=$(VERSION) $(MVN_OPTIONS) -B clean test

pre-build:
	mvn -Dgit_release=$(VERSION) $(MVN_OPTIONS) -B clean
	mvn -Dgit_release=$(VERSION) $(MVN_OPTIONS) -B package

deploy:
	@if [[ "$(VERSION)" =~ .*-dirty ]] ; then echo "refusing to deploy a dirty image." && git status -s . && exit 1 ; else exit 0; fi
	make deploy-pom
	set -e ; for DIR in $(ALL_DEPLOY); do $(MAKE) -C $$DIR deploy ; done

deploy-pom:
	mvn clean source:jar install -DcreateChecksum=true
	git clone --single-branch --branch mvn_repo git@gitlab.com:verenigingcoin-public/coin-sdk-java.git target/mvn_repo
	cp -rf ~/.m2/repository/nl/coin/coin-java-sdk-pom/* target/mvn_repo/nl/coin/coin-java-sdk-pom
	cd target/mvn_repo;	git add .; git commit -am "New release of the COIN Java SDK POM version $(VERSION)"; git push origin mvn_repo
	rm -rf target/mvn_repo
