# ProductInfo.Version.Manager

A project has a file (see attached) which describes the version number of an application. This number is of the 
following format 1.0.<major>.<minor> eg 1.0.20.3

The major or minor number needs to be incremented depending on the type of release of the application:
* For a “Feature” release the major number is incremented and the minor number is reset to zero.
* For a “Bug Fix” release the minor number is incremented.

This console application can be used in two ways:
1. The first is by calling it from command line, with the expected arguments. Doing this, it tries to open the file at 
the provided path and replace the contents with the updated version.

    ```ProductInfo.Version.Manager --path "PathToAFile" --releaseType feature/bugfix```

2. The second way is by opening the app and providing input via console, as the app is requesting it. You will first 
have to provide a file path, after which you can input one of the 3 available commands:
     * exit - exits the app
     * bugfix - increments minor version
     * feature - increments major version and reset minor version

# Possible improvements

* More verifications could be put in place for command line input.
* Similarly, more verifications could be added for the file path and to check the contents of the existing file.
* Better unit test coverage.
* Wrapping Environment.Exit() call, so it can be mocked in UnitTests.