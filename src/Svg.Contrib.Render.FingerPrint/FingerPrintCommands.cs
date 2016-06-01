﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintCommands
  {
    [NotNull]
    [ItemNotNull]
    private IDictionary<BarCodeType, string> BarCodeTypeMappings { get; } = new Dictionary<BarCodeType, string>
                                                                            {
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128, "CODE128"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128A, "CODE128A"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128B, "CODE128B"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128C, "CODE128C"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Interleaved2Of5, "INT2OF5"
                                                                              }
                                                                            };

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Position(int horizontalStart,
                                   int verticalStart)
    {
      return $"PP {horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Box(int width,
                              int height,
                              int lineWeight)
    {
      return $"PX {height},{width},{lineWeight}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Line(int length,
                               int lineWeight)
    {
      return $"PL {length},{lineWeight}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintFeed(int copies = 1)
    {
      return $"PF {copies}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintText([NotNull] string text)
    {
      return $@"PT ""{text}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Font([NotNull] string fontName,
                               int height,
                               int slant)
    {
      return $@"FT ""{fontName}"",{height},{slant}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Direction(Direction direction)
    {
      return $"DIR {(int) direction}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Align(Alignment alignment)
    {
      return $"AN {(int) alignment}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string ImageLoad([NotNull] string name,
                                    int totalNumberOfBytes)
    {
      var skip = Environment.NewLine.ToCharArray()
                            .Count() - 1;
      return $@"IMAGE LOAD {skip},""{name}"",{totalNumberOfBytes},""""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintImage([NotNull] string name)
    {
      return $@"PM ""{name}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string SelectCharacterSet(CharacterSet characterSet)
    {
      return $"NASC {characterSet.ToString("D")}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string VerbOff()
    {
      return "VERBOFF";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string InputOff()
    {
      return "INPUT OFF";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string ImmediateOn()
    {
      return "IMMEDIATE ON";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string NormalImage()
    {
      return "NI";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string InvertImage()
    {
      return "INVIMAGE";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string RemoveImage([NotNull] string name)
    {
      return $@"REMOVE IMAGE ""{name}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintBuffer(int totalNumberOfBytes)
    {
      return $"PRBUF {totalNumberOfBytes}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintBarCode([NotNull] string data)
    {
      return $@"PB ""{data}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string BarCodeMagnify(int widthFactor)
    {
      return $"BM {widthFactor}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string BarCodeHeight(int height)
    {
      return $"BH {height}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string BarCodeRatio(int wideBarFactor,
                                       decimal narrowBarFactor)
    {
      return $"BR {wideBarFactor},{narrowBarFactor}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string BarCodeType(BarCodeType barCodeType)
    {
      var barcode = this.BarCodeTypeMappings[barCodeType];

      return $@"BT ""{barcode}""";
    }
  }
}