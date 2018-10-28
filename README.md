ASP.NET Core Resiliency Kit
===========================

| Branch | Builds | Tests |
| ------ | ------ | ----- |
| master | [![AppVeyor Build Status](https://img.shields.io/appveyor/ci/Silic0nS0ldier/fallbacks-for-asp-net-core/master.svg?label=AppVeyor&style=flat-square)](https://ci.appveyor.com/project/Silic0nS0ldier/fallbacks-for-asp-net-core) [![Travis Build Status](https://img.shields.io/travis/Silic0nS0ldier/ASP.NET-Core-Resiliency-Kit/master.svg?label=Travis&style=flat-square)](https://travis-ci.org/Silic0nS0ldier/Fallbacks-for-ASP.NET-Core) | [![AppVeyor Tests Status](https://img.shields.io/appveyor/tests/Silic0nS0ldier/fallbacks-for-asp-net-core/master.svg?label=AppVeyor&style=flat-square)](https://ci.appveyor.com/project/Silic0nS0ldier/ASP.NET-Core-Resiliency-Kit) |
| dev    | [![AppVeyor Build Status](https://img.shields.io/appveyor/ci/Silic0nS0ldier/fallbacks-for-asp-net-core/dev.svg?label=AppVeyor&style=flat-square)](https://ci.appveyor.com/project/Silic0nS0ldier/fallbacks-for-asp-net-core) [![Travis Build Status](https://img.shields.io/travis/Silic0nS0ldier/ASP.NET-Core-Resiliency-Kit/dev.svg?label=Travis&style=flat-square)](https://travis-ci.org/Silic0nS0ldier/ASP.NET-Core-Resiliency-Kit) | [![AppVeyor Tests Status](https://img.shields.io/appveyor/tests/Silic0nS0ldier/fallbacks-for-asp-net-core/dev.svg?label=AppVeyor&style=flat-square)](https://ci.appveyor.com/project/Silic0nS0ldier/fallbacks-for-asp-net-core) |

A collection of utilities designed to improve resilience of sites that utilise external hosting of static resources such as CDNs.

Features
--------

1. Alternative resource sources
   Take advantage of a content delivery network without the risk of your site breaking if it goes down.
2. Potential for secure [SRI](https://en.wikipedia.org/wiki/Subresource_Integrity) hash generation
   Since the default resource (e.g. `src` for `<script />`) is pointed at your local copy, any tool that generates an integrity hash automatically will be basing it on your copy, and not the remote source that could change at any time.
   At runtime we'll make the first alternative source the default, and move the default to the end of the alternatives collection.

Quick Start
-----------

Install the package

```PowerShell
dotnet add package AspNetCoreResiliencyKit.TagHelpers
```

Reference tag helper from `cshtml` page

```C#
@using AspNetCoreResiliencyKit.TagHelpers;
@addTagHelper *, AspNetCoreResiliencyKit.TagHelpers
```

Use it!

```C#
<script src="https://local.example.com/example.css" src-alternatives="@{new List<string>(){ "https://cnd1.example.com/example.css", "https://cnd2.example.com/example.css"};}" async></script>
```

And that's it!

In terms of order, your looking at `https://cnd1.example.com/example.css` first, followed by `https://cnd2.example.com/example.css` and then `https://local.example.com/example.css`. Any existing attributes applied to the tag will remain with the exception of `onerror` which is used to invoke the alternatives. No need for tedious property checks, and complete support for `async` and `defer`.

TODO: Verify if deferred fallbacks maintain order.

Why
---

CDNs fail and without a fallback, so does your site. This isn't a new issue of course, and plenty of solutions already exist. However the work required and inherent limitations leave much to be desired.

Loaders like [`Fallback.js`](http://fallback.io/) require an extra (although small) file to be downloaded, and provides no graceful fallback should it fail itself. They also prevent the use of native `async` and `defer`, sidestepping any browser level optimisations. The solution I present here inlines the required source and hooks into the native `onerror` event for maximised stability.

Attribute tests used by the fallback system in the ASP.NET Core `ScriptTagHelper` and `LinkTagHelper` are reliable, but require much more effort and may not even be viable. Furthermore they are limited to a single fallback per resource.

In short, I wanted an easy to use and lightweight system for reliable fallbacks. Something that wouldn't require paving over the issue with a massive JS framework.

How
---

The `link` and `script` tags both have widespread support for the `onerror` event attribute, which is triggered for any error related to load failure (including integrity validation failure). My solution takes advantage of this to reliably kick-in as needed, with minimal overhead.

Building Solution
-----------------

Node JS is required for building this library, and it must be accessible from PATH. Its job is to inject the necessary JS script in minified form directly into the code, and remove it after compilation is completed. Its an extra step, but one that ensures minimal overhead and maximum productivity.
