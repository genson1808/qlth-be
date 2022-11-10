# dotnet command

* Create solution

```bash
dotnet new sln - o [name-solution]
```

* create webapi

```bash
dotnet new webapi -o [name-webapi]
```

* create library

```bash
dotnet new classlib -o [name-classlib]
```

* build

```bash
dotnet build
```

* show info solution

```bash
more .\[name-solution]
```

* add all project in folder to solution

```bash
dotnet sln add (ls -r **\*.csproj)
```

* add reference

```bash
dotnet add MISA.QLTH.API reference MISA.QLTH.BL/ MISA.QLTH.DL MISA.QLTH.Common
```


 var connectionString = "Host=127.0.0.1; Port=3307; Database=MISAQlth_Development; User Id = root; Password = root";
var sqlConnection = new MySqlConnection(connectionString);