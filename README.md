Fallbacks for ASP.NET Core
===================

A simple, clean CSS and JS fallback library for ASP.NET Core.

[![AppVeyor Build Status](
https://img.shields.io/appveyor/ci/Silic0nS0ldier/fallbacks-for-asp-net-core/dev.svg?logo=appveyor&label=Windows%20Build&style=flat-square)](https://ci.appveyor.com/project/Silic0nS0ldier/fallbacks-for-asp-net-core)
[![AppVeyor Tests Status](
https://img.shields.io/appveyor/tests/Silic0nS0ldier/fallbacks-for-asp-net-core/dev.svg?logo=appveyor&label=Windows%20Tests&style=flat-square)](https://ci.appveyor.com/project/Silic0nS0ldier/fallbacks-for-asp-net-core)
[![Travis Build Status](
https://img.shields.io/travis/Silic0nS0ldier/Fallbacks-for-ASP.NET-Core/dev.svg?label=Linux%20Build&style=flat-square)](https://travis-ci.org/Silic0nS0ldier/Fallbacks-for-ASP.NET-Core)

Features
--------

- Reliably fallback to alternative resources.
- Tracking of resource failures (original, fallbacks, and complete failures).

Quick Start
-----------

Install the package

```PowerShell
dotnet add package Fallback.AspNetCore
```

Reference tag helper from `cshtml` page

```C#
@using Fallback.AspNetCore;
@addTagHelper Fallback.AspNetCore.FallbackTagHelper
```

Use it!

```C#
<script src="www.example.com/cdn1/example.css" fallback-url="www.example.com/cdn2/example.css~./example.css" async></script>
```

And that's it!

In terms of order, your looking at `www.example.com/cdn1/example.css` first, followed by `www.example.com/cdn2/example.css` and then `./example.css`. Any existing attributes applied to the tag will remain.

Why
---

CDNs fail and without a fallback, so does your site. This isn't a new issue of course, and plenty of solutions already exist. However I always found a flaw, be it too much reliance or too much effort involved.

Loaders like [`Fallback.js`](http://fallback.io/) require an extra (although small) file to be downloaded, and provides no graceful fallback should it fail itself. My solution inlines all the required code once, and only once. They also prevent the use of native `async` and `defer`, sidestepping any browser level optimisations.

Attribute tests used by the fallback system in the ASP.NET Core `ScriptTagHelper` and `LinkTagHelper` are reliable, but require much more effort and may not even be viable.

Simply put, I wanted an easy, lightweight system that provided reliable fallbacks encouraging the adoption of solid web standards. (and not just paving over them with massive JS libraries)

How
---

The `link` and `script` tags both have widespread support for the `error` event, which is triggered for any error related to load failure. My solution takes advantage of this to reliably kick-in as needed, with minimal overhead.

Building Solution
-----------------

Node JS is required for building this library, and it must be accessible from PATH. Its job is to inject the necessary JS script in minified form directly into the code, and remove it after compilation is completed. Its an extra step, but one that ensures maximum performance.

To Do
-----

- Execute callback on failure, if supplied. - Implementation subject to user interest.
