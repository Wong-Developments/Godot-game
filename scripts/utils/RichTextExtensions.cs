using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.Utils;

public static class RichTextExtensions
{
    public static string Header(this string text) => $"[color=WHITE][[/color]{text}[color=WHITE]][/color]";
    public static string Color(this string text, string color) => $"[color={color}]{text}[/color]";
    public static string Bold(this string text) => $"[b]{text}[/b]";
    public static string Italic(this string text) => $"[i]{text}[/i]";
}
