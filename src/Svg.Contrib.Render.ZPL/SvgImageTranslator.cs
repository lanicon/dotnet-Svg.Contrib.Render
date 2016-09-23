﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<ZplContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands"/> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                              [NotNull] ZplCommands zplCommands)
      : base(zplTransformer)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmap"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void StoreGraphics([NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] ZplContainer container)
    {
      if (variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      int numberOfBytesPerRow;
      var rawBinaryData = this.ZplTransformer.GetRawBinaryData(bitmap,
                                                               false,
                                                               out numberOfBytesPerRow);

      container.Header.Add(this.ZplCommands.DownloadGraphics(variableName,
                                                             rawBinaryData,
                                                             numberOfBytesPerRow));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void GraphicDirectWrite([NotNull] SvgImage svgElement,
                                               [NotNull] Matrix sourceMatrix,
                                               [NotNull] Matrix viewMatrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               [NotNull] ZplContainer container)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      using (var bitmap = this.ZplTransformer.ConvertToBitmap(svgElement,
                                                              sourceMatrix,
                                                              viewMatrix,
                                                              (int) sourceAlignmentWidth,
                                                              (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        int numberOfBytesPerRow;
        var rawBinaryData = this.ZplTransformer.GetRawBinaryData(bitmap,
                                                                 false,
                                                                 out numberOfBytesPerRow);

        container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                         verticalStart));
        container.Body.Add(this.ZplCommands.GraphicField(rawBinaryData,
                                                         numberOfBytesPerRow));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void PrintGraphics([NotNull] SvgImage svgImage,
                                          [NotNull] Matrix sourceMatrix,
                                          [NotNull] Matrix viewMatrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          [NotNull] string variableName,
                                          [NotNull] ZplContainer container)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                       verticalStart));
      container.Body.Add(this.ZplCommands.RecallGraphic(variableName));
    }
  }
}