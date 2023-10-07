Imports System
Imports System.Windows.Data

Namespace OpenSilverBusinessApplication

    ''' <summary>
    ''' Two way <see cref="IValueConverter"/> that lets you bind the inverse of a boolean property to a dependency property.
    ''' </summary>
    Public Class NotOperatorValueConverter Inherits IValueConverter

        ''' <summary>
        ''' Converts the given <paramref name="value"/> to be its inverse.
        ''' </summary>
        ''' <param name="value">The <c>bool</c> value to convert.</param>
        ''' <param name="targetType">The type to convert to (ignored).</param>
        ''' <param name="parameter">Optional parameter (ignored).</param>
        ''' <param name="culture">The culture of the conversion (ignored).</param>
        ''' <returns>The inverse of the input <paramref name="value"/>.</returns>
        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object
            Return Not CType(value, Boolean) 
        End Function

        ''' <summary>
        ''' The inverse of the <see cref="Convert"/>.
        ''' </summary>
        ''' <param name="value">The value to convert back.</param>
        ''' <param name="targetType">The type to convert to (ignored).</param>
        ''' <param name="parameter">Optional parameter (ignored).</param>
        ''' <param name="culture">The culture of the conversion (ignored).</param>
        ''' <returns>The inverse of the input <paramref name="value"/>.</returns>
        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object
            Return Not CType(value, Boolean) 
        End Function

    End Class

End Namespace