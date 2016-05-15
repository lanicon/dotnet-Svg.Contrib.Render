﻿using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                  [NotNull] EplCommands eplCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
      this.SvgUnitReader = svgUnitReader;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    public override void Translate([NotNull] SvgRectangle svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] EplStream container)
    {
      EplStream eplStream;

      if (svgElement.Fill != SvgPaintServer.None
          && (svgElement.Fill as SvgColourServer)?.Colour != Color.White)
      {
        eplStream = this.TranslateFilledBox(svgElement,
                                            matrix);
      }
      else if (svgElement.Stroke != SvgPaintServer.None)
      {
        eplStream = this.TranslateBox(svgElement,
                                      matrix);
      }
      else
      {
        return;
      }

      if (!eplStream.IsEmpty)
      {
        container.Add(eplStream);
      }
    }

    [NotNull]
    protected virtual EplStream TranslateFilledBox([NotNull] SvgRectangle instance,
                                                   [NotNull] Matrix matrix)
    {
      var startX = this.SvgUnitReader.GetValue(instance,
                                               instance.X);
      var startY = this.SvgUnitReader.GetValue(instance,
                                               instance.Y);
      var endX = startX + this.SvgUnitReader.GetValue(instance,
                                                      instance.Width);
      var endY = startY + this.SvgUnitReader.GetValue(instance,
                                                      instance.Height);

      var svgLine = new SvgLine
                    {
                      Color = instance.Color,
                      Stroke = SvgPaintServer.None,
                      StrokeWidth = instance.StrokeWidth,
                      StartX = startX,
                      StartY = startY,
                      EndX = endX,
                      EndY = endY
                    };

      float strokeWidth;
      this.EplTransformer.Transform(svgLine,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var horizontalLength = (int) Math.Abs(endX - startX);
      var verticalLength = (int) Math.Abs(endY - startY);
      var eplStream = this.EplCommands.LineDrawBlack(horizontalStart,
                                                     verticalStart,
                                                     horizontalLength,
                                                     verticalLength);

      return eplStream;
    }

    [NotNull]
    protected virtual EplStream TranslateBox([NotNull] SvgRectangle instance,
                                             [NotNull] Matrix matrix)
    {
      float startX;
      float endX;
      float startY;
      float endY;
      float strokeWidth;
      this.EplTransformer.Transform(instance,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var lineThickness = (int) strokeWidth;
      var horizontalEnd = (int) endX;
      var verticalEnd = (int) endY;
      var eplStream = this.EplCommands.DrawBox(horizontalStart,
                                               verticalStart,
                                               lineThickness,
                                               horizontalEnd,
                                               verticalEnd);

      return eplStream;
    }
  }
}