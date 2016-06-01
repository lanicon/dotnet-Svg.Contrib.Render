﻿using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<FingerPrintContainer, T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    public SvgTextBaseTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                 [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] FingerPrintContainer container)
    {
      if (svgElement.Text == null)
      {
        return;
      }

      var text = this.RemoveIllegalCharacters(svgElement.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
        return;
      }

      float fontSize;
      int horizontalStart;
      int verticalStart;
      Direction direction;
      this.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
                       out horizontalStart,
                       out verticalStart,
                       out fontSize,
                       out direction);

      string fontName;
      int characterHeight;
      int slant;
      this.FingerPrintTransformer.GetFontSelection(svgElement,
                                                   fontSize,
                                                   out fontName,
                                                   out characterHeight,
                                                   out slant);

      this.AddTranslationToContainer(svgElement,
                                     horizontalStart,
                                     verticalStart,
                                     direction,
                                     fontName,
                                     characterHeight,
                                     slant,
                                     text,
                                     container);
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual string RemoveIllegalCharacters([NotNull] string text)
    {
      // TODO add regex for removing illegal characters ...

      // ReSharper disable ExceptionNotDocumentedOptional
      return text.Replace("\"",
                          "'");
      // ReSharper restore ExceptionNotDocumentedOptional
    }

    [Pure]
    protected virtual void GetPosition([NotNull] T svgElement,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out float fontSize,
                                       out Direction direction)
    {
      float x;
      float y;
      this.FingerPrintTransformer.Transform(svgElement,
                                            sourceMatrix,
                                            viewMatrix,
                                            out x,
                                            out y,
                                            out fontSize,
                                            out direction);

      horizontalStart = (int) x;
      verticalStart = (int) y;
    }

    protected virtual void AddTranslationToContainer([NotNull] T svgElement,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     Direction direction,
                                                     [NotNull] string fontName,
                                                     int characterHeight,
                                                     int slant,
                                                     [NotNull] string text,
                                                     [NotNull] FingerPrintContainer container)
    {
      container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      container.Body.Add(this.FingerPrintCommands.Direction(direction));
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.BaseLineLeft));

      if ((svgElement.Fill as SvgColourServer)?.Colour == Color.White)
      {
        container.Body.Add(this.FingerPrintCommands.InvertImage());
      }
      else
      {
        container.Body.Add(this.FingerPrintCommands.NormalImage());
      }

      container.Body.Add(this.FingerPrintCommands.Font(fontName,
                                                       characterHeight,
                                                       slant));
      container.Body.Add(this.FingerPrintCommands.PrintText(text));
    }
  }
}