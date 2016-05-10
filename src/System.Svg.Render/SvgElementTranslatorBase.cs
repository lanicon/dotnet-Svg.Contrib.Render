﻿using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgElementTranslatorBase<T> : ISvgElementTranslator<T>
    where T : SvgElement
  {
    protected SvgElementTranslatorBase([NotNull] ISvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    public bool TryTranslateUntyped([NotNull] object untypedInstance,
                                    [NotNull] Matrix matrix,
                                    int targetDpi,
                                    out object translation)
    {
      var success = this.TryTranslate((T) untypedInstance,
                                      matrix,
                                      targetDpi,
                                      out translation);

      return success;
    }

    public abstract bool TryTranslate([NotNull] T instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation);
  }
}