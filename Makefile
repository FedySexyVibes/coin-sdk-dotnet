include Makefile.mk

ALL=common-sdk number-portability-sdk number-portability-sdk-samples

tag-all-patch-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-patch-release ; done

tag-all-minor-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-minor-release ; done

tag-all-major-release:
	set -e ; for DIR in $(ALL); do $(MAKE) -C $$DIR tag-major-release ; done

