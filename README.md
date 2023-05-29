# LibCLCC.NET

Creeper Lv's Common Components Library.

## Installation

The library is currently published to [NuGet.org](https://www.nuget.org/packages/LibCLCC.NET/), you can install it manually download and install it from there.

Or, you can install it by:

```
dotnet add package LibCLCC.NET
```

## Goal

The goal of this library is to make life easier and reduce rewriting every code every time a new project/tool is created.

Also, the library is aimed on providing functions in pure C# and compatible with Unity3D (at least the main library).

## Features

| Name			| Description |
| ---  | --- |
|ConnectableList			| Help concatenate two lists |
|ChainAction				| A Chain Action|
|BreakableFunc				| A breakable function chain, returning `true` will break the chain|
|ObservableData				| A wrapper of data that will invoke breakable functions when the data is changed |
|ObservableDisposableData	| A wrapper of disposable data that will invoke breakable functions when the data is changed |
|KVPair						| A key-value pair |
|KVList						| A list which T is KVPair |
|ReactableList				| A list that invoke BreakableFunc for Add,Remove,RemoveAt,Clear|
|GeneralPurposeScanner		| A general purpose scanner. |

## License

The project is licensed under the MIT License.