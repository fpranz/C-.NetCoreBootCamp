# DAY 3 Lesson 
## .Net Core using Bootwatch
### Razor Syntax

Razor supports C# and uses the `@` symbol to transition from HTML to C#. Razor evaluates C# expressions and renders them in the HTML output.


## Implicit Razor Expressions

Implicit Razor expressions start with `@` followed by C# code:

```cshtml
<p>@DateTime.Now</p>
<p>@DateTime.IsLeapYear(2016)</p>
```

## Explicit Razor Expression

Explicit Razor expressions consist of an `@` symbol with balanced parenthesis. To render last week's time, the following Razor markup is used:

```cshtml
<p>Last week this time: @(DateTime.Now - TimeSpan.FromDays(7))</p>
```

Any content within the @() parenthesis is evaluated and rendered to the output.

Implicit expressions, described in the previous section, generally can't contain spaces. In the following code, one week isn't subtracted from the current time:

```cshtml
<p>Last week: @DateTime.Now - TimeSpan.FromDays(7)</p>
```

The code renders the following HTML:

```html
<p>Last week: 7/7/2016 4:39:52 PM - TimeSpan.FromDays(7)</p>
```

Explicit expressions can be used to concatenate text with an expression result:

```cshtml
@{
    var joe = new Person("Joe", 33);
}

<p>Age@(joe.Age)</p>
```

`HtmlHelper.Raw` output isn't encoded but rendered as HTML markup.

```cshtml
@Html.Raw("<span>Hello World</span>")
```

## Razor code blocks

Razor code blocks start with `@` and are enclosed by `{}`. Unlike expressions, C# code inside code blocks isn't rendered. Code blocks and expressions in a view share the same scope and are defined in order:

```cshtml
@{
    var quote = "The future depends on what you do today. - Mahatma Gandhi";
}

<p>@quote</p>

@{
    quote = "Hate cannot drive out hate, only love can do that. - Martin Luther King, Jr.";
}

<p>@quote</p>
```

The code renders the following HTML:

```html
<p>The future depends on what you do today. - Mahatma Gandhi</p>
<p>Hate cannot drive out hate, only love can do that. - Martin Luther King, Jr.</p>
```

In code blocks, declare local functions with markup to serve as templating methods:

```cshtml
@{
    void RenderName(string name)
    {
        <p>Name: <strong>@name</strong></p>
    }

    RenderName("Mahatma Gandhi");
    RenderName("Martin Luther King, Jr.");
}
```

### [Bootswatch](https://bootswatch.com/default/)


## Creating a submit form

### Sample Model

```cs
// filename: User.cs
namespace SampleWeb.Models{
    public class User{
        public string?  FullName { get; set; }
        public int Age { get; set; }
    }
}
```

### Sample Controller

```cs
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SampleWeb.Models;

namespace SampleWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        User user = new User();
        user.FullName = "Jomiel L. Enriquez";
        return View(user);
    }
    public IActionResult Test(User user){
        return RedirectToAction("Index");
    }
}

```

### Sample View 

```cshtml
@model SampleWeb.Models.User

@{
    ViewData["Title"] = "Home Page";
}


@using (Html.BeginForm("Test", "Home", FormMethod.Post, new { @name = "testForm" })){
    
    @Html.TextBoxFor(model => model.FullName, new { @class = "form-control", @id = "FullName" })

    @Html.TextBoxFor(model => model.Age, new { @class = "form-control", @id = "Age" })

    <button type="submit" class="btn btn-success" href=""><i class="icon-save"></i>  Save</button>
}
```

# Creating Dropdown

```cshtml
<div class="form-group row margin_bottom_10">
    <label for="Name" class="col-sm-2 col-form-label">Barangay</label>
    <div class="col-sm-10">
        @{
            var users = new List<User>();
            users.Add(new User(){FullName="test",Age=1});
            users.Add(new User(){FullName="test1",Age=2});

            var selection = new List<SelectListItem>();
            foreach (var user in users)
            {
                selection.Add(new SelectListItem { Text = user.FullName, Value = user.Age.ToString() });
            }
        }
        @Html.DropDownListFor(model => model.Age, selection, "Select", new { @class = "form-select", @id = "Test" })
    </div>
</div>
```

# First Example
## Model
```cs
namespace SampleWeb.Models{
    public class Person{
        public Person(string name, int age){
            this.Name = name;
            this.Age = age;
        }
        public string?  Name { get; set; }
        public int Age { get; set; }
    }
}
```

### Index
```cshtml

@{
    ViewData["Title"] = "Home Page";
}

@{
    var joe = new Person("Joe", 33);
}

<p>Name: @joe.Name</p>
<p>Age: @joe.Age</p>

```

# Display table of Persons

```cshtml

@{
    ViewData["Title"] = "Home Page";
}
@{
    var listOfPerson = new List<Person>();
    
    listOfPerson.Add(new Person("Jomiel", 26));
    listOfPerson.Add(new Person("Frank", 22));
    listOfPerson.Add(new Person("Ronald", 25));
    listOfPerson.Add(new Person("Mico", 65));
    listOfPerson.Add(new Person("Fernando", 69));
    listOfPerson.Add(new Person("Maki", 16));
    listOfPerson.Add(new Person("Jeffrey", 16));
    listOfPerson.Add(new Person("Franz", 1));
}
<table class="table table-hover">
    <thead>
        <tr>
            <td>Name</td>
            <td>Age</td>
        </tr>
    </thead>
    <tbody>
        @{
            foreach(var person in listOfPerson){
                <tr>
                    <td>@person.Name</td>
                    <td>@person.Age</td>
                </tr>
            }
        }
    </tbody>
</table>
```