' “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍
Imports 五行查询器.Extension
Imports 五行查询器.Math
''' <summary>
''' 可用于自身或导航至 Frame 内部的空白页。
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    ''' <summary>
    ''' 在此页将要在 Frame 中显示时进行调用。
    ''' </summary>
    ''' <param name="e">描述如何访问此页的事件数据。Parameter
    ''' 属性通常用于配置页。</param>
    Protected Overrides Sub OnNavigatedTo(e As Navigation.NavigationEventArgs)
    
    End Sub

    Private Sub DatePicker_Open(sender As Object, e As RoutedEventArgs)
        Dim d As New DatePicker
        d.ShowDatePicker()
        AddHandler d.Complete, Sub(r, ev)
                                   If Not ev.Cancel Then txt_bazi.Text = DateTimeHelper.ComputeGan(r)
                               End Sub
    End Sub

    Private Sub Query_Click(sender As Object, e As RoutedEventArgs)
        txt_result.Text = BaziAlgorithm.EvalBazi(txt_bazi.Text)
    End Sub
End Class
