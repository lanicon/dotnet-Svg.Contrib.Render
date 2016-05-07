# dotnet-System.Svg.Render.EPL

This project aims to provide a translation/compilation/transpilation of SVG to EPL. You can read *.svg*-files with `System.Svg.SvgDocument.Open(path:string)`.

It references [System.Svg](https://github.com/dittodhole/dotnet-System.Svg), a fork of [SVG.NET](https://github.com/vvvv/SVG) without all the actual image-rendering bloat. Additionally, [ExCSS](https://github.com/TylerBrinks/ExCSS) and [Fizzler](https://code.google.com/archive/p/fizzler) are references instead.

## Getting started

You can start by using the `System.Svg.Render.EPL.DefaultBootstrapper`:

```
var sourceDpi = 90; // default of Inkscape
var targetDpi = 203; // default dpi for EPL printers
var file = "";
var svgDocument = System.Svg.SvgDocument.Open(file);
var svgDocumentTranslator = System.Svg.Render.EPL.DefaultBootstrapper.Create(sourceDpi,
                                                                             SvgUnitType.Pixel);
var translation = svgDocumentTranslator.Translate(svgDocument,
                                                  targetDpi);
// TADADADA
```

## I am interested, tell me more ...

### Currently implemented elements

- `<text>` and `<tspan>` w/ white stroke for inverting text
- `<group>` (to define a transformation for a whole group of elements)
- `<rectangle>` (with white filling for inverting text)
- `<line>`
- `<svg>` (if any transformation exists on the root)

### Currently implemented attributes

- `transform`
- `text`
- `x`
- `x1` (`<line>`)
- `x2` (`<line>`)
- `y`
- `y1` (`<line>`)
- `y2` (`<line>`)
- `width`
- `height`
- content of `<text>` or `<tspan>`

### Currently implemented styles
- `fill` (`<rectangle>`) w/ solid color
- `stroke` (`<rectangle>`, `<line>`, `<text>`) w/ solid color
- `stroke-width`
- `visible`

## Installing [![NuGet Status](http://img.shields.io/nuget/v/System.Svg.Render.EPL.svg?style=flat)](https://www.nuget.org/packages/System.Svg.Render.EPL/)

tbd

## License

dotnet-System.Svg.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).
