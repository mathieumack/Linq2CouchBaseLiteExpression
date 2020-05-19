# Linq2CouchBaseLiteExpression
Linq extension to query couchbase lite repository (Linq to Expression)

Writen in .net standard 2.0

# IC
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=github-Linq2CouchBaseLiteExpression&metric=alert_status)](https://sonarcloud.io/dashboard?id=github-Linq2CouchBaseLiteExpression)
[![Build status](https://dev.azure.com/mackmathieu/Github/_apis/build/status/Linq2CouchBaseLiteExpression)](https://dev.azure.com/mackmathieu/Github/_build/latest?definitionId=17)
[![NuGet version](https://badge.fury.io/nu/Linq2CouchBaseLiteExpression.svg)](https://badge.fury.io/nu/Linq2CouchBaseLiteExpression)

# How to :

The class Linq2CouchbaseLiteExpression contains a unique method named GenerateFromExpression that generate an couchbase lite IExpression.

This generic method works on objects that are class.

First of all, install the plugin by the nuget package.

## Example

```csharp

/// <summary>
/// Entity object
/// </summary>
public class EntityObject
{
    public string Id { get; set; }

    public string Name { get; set; }

	public int Value { get; set; }
	
	public bool IsHuman { get; set; }
}

```
```csharp

// Generate IExpression from a Lambda expression :
var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name == "test");

```

This call will generate a Couchbase lite expression like this :

```csharp

// Generate manually the IExpression :
Couchbase.Lite.Query.Expression.Property("Name").EqualTo(Couchbase.Lite.Query.Expression.String("test"))

```

## Supported operations
These operations are now supported

Function | Example
--- | ---
\> |  <pre lang=csharp> (e) => e.Value > 10</pre>
\< |  <pre lang=csharp> (e) => e.Value < 10</pre>
\>= |  <pre lang=csharp> (e) => e.Value >= 10</pre>
\>= |  <pre lang=csharp> (e) => e.Value <= 10</pre>
== |  <pre lang=csharp> (e) => e.Name == "test"</pre>
!= |  <pre lang=csharp> (e) => e.Name != "test"</pre>
! |  <pre lang=csharp> (e) => !e.IsHuman</pre>
.Equals | <pre lang=csharp> (e) => e.Name.Equals("test")</pre>
string.IsNullOrEmpty | <pre lang=csharp> (e) => string.IsNullOrEmpty(e.Name)</pre>

You can also combine conditions :

Function | Example
--- | ---
\|\| | <pre lang=csharp> (e) => e.Name == "test" \|\| e.Name == "test 2"</pre>
\&\& | <pre lang=csharp> (e) => e.Value > 10 \&\& e.Name == "test 2"</pre>
</pre>

## Lambda expression writing

The expression must respect some specific rules :
* You can set field object at left or at right of the operation :
```csharp

// Valid :
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name == "test");

// Or :
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => "test" = e.Name );

```

* You can use call static method withtout parameters only :
```csharp

// Valid :
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name == CallToStaticMethod());

// Invalid :
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name == CallToStaticMethod("Parameter"));

// Invalid :
var customObject = new CustomObject();
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name ==  customObject.NonPublicMethod());

// Invalid :
var customObject = new CustomObject();
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name ==  customObject.NonPublicMethodWithParameters("test"));

```

* Only the methods are now supported : .Equals() or string.IsNullOrEmpty()
```csharp

// Valid :
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => e.Name.Equals("test"));

// Valid :
Linq2CouchbaseLiteExpression.GenerateFromExpression<EntityObject>((e) => string.IsNullOrEmpty(e.Name);

```

# Contribute

You want more ? Feel free to create an issue or contribute by adding new functionnalities by forking the project and create a pull request.