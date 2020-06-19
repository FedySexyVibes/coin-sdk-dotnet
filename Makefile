include Makefile.mk

ALL=common-sdk number-portability-sdk number-portability-sdk-samples bundle-switching-sdk bundle-switching-sdk-samples

tag-all-patch-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-patch-release ; done

tag-all-minor-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-minor-release ; done

tag-all-major-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-major-release ; done

publish-nuget:
	docker build -t publish-dotnet-sdk -f publish-Dockerfile . && docker run --rm -v ~/.aws:/root/.aws:ro -e AWS_CONTAINER_CREDENTIALS_RELATIVE_URI=$$AWS_CONTAINER_CREDENTIALS_RELATIVE_URI publish-dotnet-sdk
	