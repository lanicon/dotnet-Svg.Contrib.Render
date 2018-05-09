﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<EplContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
      : base(eplTransformer)
    {
      this.EplTransformer = eplTransformer ?? throw new ArgumentNullException(nameof(eplTransformer));
      this.EplCommands = eplCommands ?? throw new ArgumentNullException(nameof(eplCommands));
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="variableName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmap" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected override void StoreGraphics([NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] EplContainer eplContainer)
    {
      if (variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      var pcxByteArray = this.EplTransformer.ConvertToPcx(bitmap);

      eplContainer.Header.Add(this.EplCommands.DeleteGraphics(variableName));
      eplContainer.Header.Add(this.EplCommands.DeleteGraphics(variableName));
      eplContainer.Header.Add(this.EplCommands.StoreGraphics(variableName,
                                                             pcxByteArray.Length));
      eplContainer.Header.Add(pcxByteArray);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected override void GraphicDirectWrite([NotNull] SvgImage svgImage,
                                               [NotNull] Matrix sourceMatrix,
                                               [NotNull] Matrix viewMatrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               [NotNull] EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      using (var bitmap = this.EplTransformer.ConvertToBitmap(svgImage,
                                                              sourceMatrix,
                                                              viewMatrix,
                                                              (int) sourceAlignmentWidth,
                                                              (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        var rawBinaryData = this.EplTransformer.GetRawBinaryData(bitmap,
                                                                 true,
                                                                 out var numberOfBytesPerRow);
        var rows = bitmap.Height;

        eplContainer.Body.Add(this.EplCommands.GraphicDirectWrite(horizontalStart,
                                                                  verticalStart,
                                                                  numberOfBytesPerRow,
                                                                  rows));
        eplContainer.Body.Add(rawBinaryData);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected override void PrintGraphics([NotNull] SvgImage svgImage,
                                          [NotNull] Matrix sourceMatrix,
                                          [NotNull] Matrix viewMatrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          [NotNull] string variableName,
                                          [NotNull] EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      eplContainer.Body.Add(this.EplCommands.PrintGraphics(horizontalStart,
                                                           verticalStart,
                                                           variableName));
    }
  }
}
