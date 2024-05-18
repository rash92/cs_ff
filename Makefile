.PHONY: test
.PHONY: nuget

REPO_BRANCH = $(shell git branch --show-current)

BRANCH_PREFIX = $(patsubst %-%,%,$(REPO_BRANCH))


test:
	@echo "This tests the makefile, not the dotnet project"
	@echo $(REPO_BRANCH)
	@echo $(BRANCH_PREFIX)

dotnet_test:
	dotnet test -c Release

nuget:
	@echo $(REPO_BRANCH)
	dotnet pack -c Release -o . /p:Version=1.2.3-test