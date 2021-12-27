OutputDir:=nugets
Projects:=src/Polpware.Runtime.Serialization/Polpware.Runtime.Serialization.csproj \
src/Polpware.IO.Serialization/Polpware.IO.Serialization.csproj

ProjectNames:=$(basename $(Projects))
Nugets:=$(addsuffix .nupkg,$(ProjectNames))

%.nupkg:
	@echo "Build $@"
ifndef Config
	@echo "Please provide the config with Config=debug or Config=release"
else
ifeq ($(Config),Release)
	@echo "Output release version"
	dotnet pack $(addsuffix .csproj,$(basename $@)) --output $(OutputDir) --configuration Release
else
	@echo "Output debug version"
	dotnet pack $(addsuffix .csproj,$(basename $@)) --include-symbols --output $(OutputDir)
endif
endif


build: $(Nugets)
	@echo "********************************"
	@echo "Build done"
	@echo "********************************"

pre-build:
	@echo "Create nugets dir"
	@mkdir nugets
	@echo "Done"

clean:
	@echo "Delete nugets"
	if [ -d nugets ]; then \
	  rm -rf nugets; \
        fi 
	@echo "Done"

push:
	@echo "Push to GitHub"
	git push

.PHONY: build pre-build clean push

