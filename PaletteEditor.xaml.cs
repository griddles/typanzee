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
        tertiary.Text = globalContext.settings.tertiary;
        background.Text = globalContext.settings.background;
    }

    public void UpdateColors() // there's probably a better way to do this but it works
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
        try
        {
            tertiary.Background = (Brush)new BrushConverter().ConvertFromString(tertiary.Text)!;
            tertiary.Foreground = GetContrast(tertiary.Background);
            globalContext.settings.tertiary = tertiary.Text;
        }
        catch
        {
            tertiary.Background = Brushes.Black;
            tertiary.Foreground = Brushes.Red;
        }
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

    public static Brush GetContrast(Brush brushColor)
    {
        string stringColor = brushColor.ToString();
        Color color = ColorTranslator.FromHtml(stringColor);
        float brightness = color.GetBrightness();
        
        return brightness > 0.5 ? Brushes.Black : Brushes.White;
    }

    private void primary_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (background != null) // this runs when primary gets initialized, which is before background gets initialized, so make sure that it only runs when background is initialized
        {
            UpdateColors();
        }
    }

    private void secondary_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (background != null)
        {
            UpdateColors();
        }
    }

    private void tertiary_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (background != null)
        {
            UpdateColors();
        }
    }

    private void background_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (background != null)
        {
            UpdateColors();
        }
    }
}