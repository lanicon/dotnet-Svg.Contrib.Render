﻿using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public class ZplCommands
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplStream CreateZplStream() => new ZplStream();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string FieldOrigin(int horizontalStart,
                                      int verticalStart)
    {
      return $"^FO{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string FieldTypeset(int horizontalStart,
                                       int verticalStart)
    {
      return $"^FT{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string GraphicBox(int width,
                                     int height,
                                     int thickness,
                                     LineColor lineColor)
    {
      return $"^GB{width},{height},{thickness},{(char) lineColor}^FS";
    }

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //public virtual string GraphicDiagonalLine(int width,
    //                                          int height,
    //                                          int thickness,
    //                                          LineColor lineColor,
    //                                          Orientation orientation)
    //{
    //  return $"^GD{width},{height},{thickness},{(char) lineColor},{(char) orientation}^FS";
    //}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Font([NotNull] string fontName,
                               FieldOrientation fieldOrientation,
                               int characterHeight,
                               int width,
                               string text)
    {
      return $"^A{fontName}{(char) fieldOrientation},{characterHeight},{width}^FD{text}^FS";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string StartFormat()
    {
      return "^XA";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string EndFormat()
    {
      return "^XZ";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintOrientation(PrintOrientation printOrientation)
    {
      return $"^PO{(char) printOrientation}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string LabelHome(int horizontalStart,
                                    int verticalStart)
    {
      return $"^LH{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string ChangeInternationalFont(CharacterSet characterSet)
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      return $"^CI{characterSet.ToString("D")}";
      // ReSharper restore ExceptionNotDocumentedOptional
    }
  }
}