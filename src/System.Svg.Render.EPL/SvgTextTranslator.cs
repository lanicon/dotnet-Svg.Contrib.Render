﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextTranslator : SvgElementTranslator<SvgText>
  {
    // TODO translate dX and dY

    public SvgTextTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

    public override bool TryTranslate(SvgText instance,
                                      Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      var svgTextSpans = instance.Children.OfType<SvgTextSpan>()
                                 .ToArray();
      if (svgTextSpans.Any())
      {
        ICollection<object> translations = new LinkedList<object>();
        foreach (var svgTextSpan in svgTextSpans)
        {
          if (!this.TryTranslate(svgTextSpan,
                                 matrix,
                                 targetDpi,
                                 out translation))
          {
            return false;
          }
          if (translation != null)
          {
            translations.Add(translation);
          }
        }

        if (translations.Any())
        {
          translation = string.Join(Environment.NewLine,
                                    translations);
        }
        else
        {
          translation = null;
        }

        return true;
      }

      var success = this.TryTranslate(instance,
                                      matrix,
                                      targetDpi,
                                      out translation);

      return success;
    }

    private bool TryTranslate([NotNull] SvgTextBase instance,
                              [NotNull] Matrix matrix,
                              int targetDpi,
                              out object translation)
    {
      var text = this.RemoveIllegalCharacters(instance.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
        translation = null;
        return true;
      }

      var rotationTranslation = this.SvgUnitCalculator.GetRotationTranslation(matrix);

      if (instance.X == null)
      {
#if DEBUG
        translation = $"; x is null: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (!instance.X.Any())
      {
#if DEBUG
        translation = $"; no x-coordinates: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (instance.Y == null)
      {
#if DEBUG
        translation = $"; y is null: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (!instance.Y.Any())
      {
#if DEBUG
        translation = $"; no y-coordinates: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      SvgUnit newX;
      SvgUnit newY;

      var x = instance.X.First();
      var y = instance.Y.First();
      if (!this.SvgUnitCalculator.TryApplyMatrix(x,
                                                 y,
                                                 matrix,
                                                 out newX,
                                                 out newY))
      {
#if DEBUG
        translation = $"; could not apply matrix on x and y: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int horizontalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newX,
                                                     targetDpi,
                                                     out horizontalStart))
      {
#if DEBUG
        translation = $"; could not get device points (x): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int verticalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newY,
                                                     targetDpi,
                                                     out verticalStart))
      {
#if DEBUG
        translation = $"; could not get device points (y): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      object fontTranslation;
      if (!this.SvgUnitCalculator.TryGetFontTranslation(instance,
                                                        matrix,
                                                        targetDpi,
                                                        out fontTranslation))
      {
#if DEBUG
        translation = $"; could not get font translation: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      string reverseImage;
      if ((instance.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = "R";
      }
      else
      {
        reverseImage = "N";
      }

      translation = $@"A{horizontalStart},{verticalStart},{rotationTranslation},{fontTranslation},{reverseImage},""{text}""";

      return true;
    }

    private string RemoveIllegalCharacters(string text)
    {
      // TODO add regex for removing illegal characters ...

      return text;
    }
  }
}