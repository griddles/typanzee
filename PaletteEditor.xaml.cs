using System;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using Color = System.Drawing.Color;

namespace typanzee;

public partial class PaletteEditor : Window
{
    public PaletteEditor()
    {
        InitializeComponent();

        primary.Text = globalContext.settings.primary;
        secondary.Text = globalContext.settings.secondary;
        dimmed.Text = globalContext.settings.dimmed;
        accent.Text = globalContext.settings.accent;
        background.Text = globalContext.settings.background;
    }

    public static Brush GetContrast(Brush brushColor)
    {
        string stringColor = brushColor.ToString();
        Color color = ColorTranslator.FromHtml(stringColor);
        float brightness = color.GetBrightness();
        
        return brightness > 0.5 ? Brushes.Black : Brushes.White;
    }

    private void primary_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        try
        {
            primary.Background = (Brush)new BrushConverter().ConvertFromString(primary.Text)!;
            primary.Foreground = GetContrast(primary.Background);
            globalContext.settings.primary = primary.Text;

        }
        catch
        {
            primary.Background = Brushes.Black;
            primary.Foreground = Brushes.Red;
        }
    }

    private void secondary_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        try
        {
            secondary.Background = (Brush)new BrushConverter().ConvertFromString(secondary.Text)!;
            secondary.Foreground = GetContrast(secondary.Background);
            globalContext.settings.secondary = secondary.Text;
        }
        catch
        {
            secondary.Background = Brushes.Black;
            secondary.Foreground = Brushes.Red;
        }
    }

    private void dimmed_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        try
        {
            dimmed.Background = (Brush)new BrushConverter().ConvertFromString(dimmed.Text)!;
            dimmed.Foreground = GetContrast(dimmed.Background);
            globalContext.settings.dimmed = dimmed.Text;
        }
        catch
        {
            dimmed.Background = Brushes.Black;
            dimmed.Foreground = Brushes.Red;
        }
    }

    private void accent_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        try
        {
            accent.Background = (Brush)new BrushConverter().ConvertFromString(accent.Text)!;
            accent.Foreground = GetContrast(accent.Background);
            globalContext.settings.accent = accent.Text;
        }
        catch
        {
            dimmed.Background = Brushes.Black;
            dimmed.Foreground = Brushes.Red;
        }
    }

    private void background_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        try
        {
            background.Background = (Brush)new BrushConverter().ConvertFromString(background.Text)!;
            background.Foreground = GetContrast(background.Background);
            globalContext.settings.background = background.Text;
        }
        catch
        {
            background.Background = Brushes.Black;
            background.Foreground = Brushes.Red;
        }
    }
}