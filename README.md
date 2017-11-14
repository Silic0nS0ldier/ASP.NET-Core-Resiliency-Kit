Fallback.AspNetCore
===================

CDNs are great, but (usually) outside your control. Fallback systems exist, but tend to hinder flexibility by requiring a check of some known property (such as the fallback feature of LinkTagHelper in ASP.Net Core) or simply force an independent JS script to handle everything (such as RequireJS, and FallbackJS).

**Fallback.AspNetCore** puts ASP.Net Core's tag helper system to work for you. Simply provide the alternative source(s) for any external script or link tag. That's it. The necessary script (weighing in at a tiny xKB) will be inserted once just before the first place a fallback is provided. Even better, you can arrange for reporting on failure, and execute your own script on failure, making it easier to track issues as they pop up.

Admittedly, there are some fantastic JS loaders out there. Whats best is really depends on your project.

To Do
-----

- Core functionality
- Phoning home on failure (log to url if specified)
- Tap a friend on failure (execute callback if specified)
- Unit testing
