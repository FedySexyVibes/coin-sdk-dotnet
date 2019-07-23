include Makefile.mk

ALL=common-sdk number-portability-sdk number-portability-sdk-samples

tag-patch-release: VERSION := $(shell . $(RELEASE_SUPPORT); nextPatchLevel)
tag-patch-release: tag

tag-minor-release: VERSION := $(shell . $(RELEASE_SUPPORT); nextMinorLevel)
tag-minor-release: tag

tag-major-release: VERSION := $(shell . $(RELEASE_SUPPORT); nextMajorLevel)
tag-major-release: tag

tag: TAG=$(shell . $(RELEASE_SUPPORT); getTag $(VERSION))
tag: check-status
	@. $(RELEASE_SUPPORT) ; ! tagExists $(TAG) || (echo "ERROR: tag $(TAG) for version $(VERSION) already tagged in git" >&2 && exit 1) ;
	@. $(RELEASE_SUPPORT) ; setRelease $(VERSION)
	find . -name "SdkInfo.cs" | xargs sed -i.tmp -E "s/(coin-sdk-dotnet)-[0-9]+\.[0-9]+\.[0-9]+/\1-$(VERSION)/"
	find . -type f -name "*.tmp" | xargs rm
	git add .
	git commit -m "bumped to version $(VERSION)" ;
	git tag $(TAG) ;
	@ if [ -n "$(shell git remote -v)" ] ; then git push ; git push --tags ; else echo 'no remote to push tags to' ; fi
