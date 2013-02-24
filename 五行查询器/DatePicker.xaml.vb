' “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上有介绍
Imports 五行查询器.Extension

Public NotInheritable Class DatePicker
    Inherits UserControl

    Private _day As DateInfo
    Private _month As DateInfo
    Private _year As DateInfo
    Private _hour As DateInfo
    Private _date As New [Date]()
    Private _iscancel As New DatePickerCompleteEventArgs
    Public Delegate Sub SelectDateComplete(result As DateTime, e As DatePickerCompleteEventArgs)
    Public Event Complete As SelectDateComplete

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        _year = New DateInfo() With {
            .Value = DateTime.Now.Year,
            .Info = GetYearName(DateTime.Now.Year)
        }
        _month = New DateInfo() With {
             .Value = DateTime.Now.Month,
             .Info = GetMonthName(DateTime.Now.Month)
        }
        _day = New DateInfo() With {
             .Value = DateTime.Now.Day,
             .Info = GetDayName(DateTime.Now.Day)
        }
        _hour = New DateInfo() With {
             .Value = DateTime.Now.Day,
             .Info = GetHourName(DateTime.Now.Hour)
        }

        Dim year As Integer = DateTime.Now.Year - 100
        _date.Day = RangeToDay(DateTime.Parse(_year.Value & "-" & _month.Value))
        _date.Month = RangeToMonth()
        _date.Year = RangeToYear(year)
        _date.Hour = RangeToHour
    End Sub

    Private Sub DatePicker_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim year = (From a In _date.Year Where a.Value = DateTime.Now.Year).FirstOrDefault()
        cb_Year.SelectedItem = year
        Dim month = (From a In _date.Month Where a.Value = DateTime.Now.Month).FirstOrDefault()
        cb_Month.SelectedItem = month
        Dim day = (From a In _date.Day Where a.Value = DateTime.Now.Day).FirstOrDefault()
        cb_Day.SelectedItem = day
        Dim hour = (From a In _date.Hour Where a.Value = DateTime.Now.Hour).FirstOrDefault()
        cb_Hour.SelectedItem = hour
    End Sub

    Private Function RangeToDay([date] As DateTime) As ObservableCollection(Of DateInfo)
        Dim valueData As New ObservableCollection(Of Integer)(Enumerable.Range(1, DateTime.DaysInMonth([date].Year, [date].Month)))
        Dim data As New ObservableCollection(Of DateInfo)()

        For Each d In valueData
            data.Add(New DateInfo() With {
                 .Value = d,
                 .Info = GetDayName([date].Day)
            })
            '多加一个防止非平板显示不了
            [date] = [date].AddDays(1)
        Next
        data.Add(data(0))
        Return data
    End Function

    Private Function RangeToMonth() As ObservableCollection(Of DateInfo)
        Dim data As New ObservableCollection(Of DateInfo)()
        For i As Integer = 1 To 12
            data.Add(New DateInfo() With {
                .Value = i,
                .Info = GetMonthName(i)
            })
        Next
        data.Add(data(0))
        '多加一个防止非平板显示不了
        Return data
    End Function

    Private Function RangeToYear(year As Integer) As ObservableCollection(Of DateInfo)
        Dim valueData As New ObservableCollection(Of Integer)(Enumerable.Range(year, 200))
        Dim data As New ObservableCollection(Of DateInfo)()
        For Each d In valueData
            data.Add(New DateInfo() With {
                .Value = d,
                .Info = GetYearName(d)
            })
        Next
        data.Add(data(0))
        '多加一个防止非平板显示不了
        Return data
    End Function

    Public Function GetDayName(day As Integer) As String
        Return DateTimeHelper.ComputeDayGan(New DateTime(_year.Value, _month.Value, day))
    End Function

    Public Function GetMonthName(month As Integer) As String
        Return DateTimeHelper.ComputeMonthGan(New DateTime(_year.Value, month, 1))
    End Function

    Public Property MyDate As [Date]
        Get
            Return _date
        End Get
        Set(value As [Date])
            _date = value
        End Set
    End Property

    Public Property Day() As DateInfo
        Get
            Return _day
        End Get
        Set(value As DateInfo)
            If _day Is value Then
                Return
            End If
            _day = value
            NotifyPropertyChanged("Day")
        End Set
    End Property

    Public Property Hour() As DateInfo
        Get
            Return _hour
        End Get
        Set(value As DateInfo)
            If _hour Is value Then
                Return
            End If
            _hour = value
            NotifyPropertyChanged("Hour")
        End Set
    End Property

    Public Property Month() As DateInfo
        Get
            Return _month
        End Get
        Set(value As DateInfo)
            If _month Is value Then
                Return
            End If
            _month = value
            _date.Day = RangeToDay(DateTime.Parse(_year.Value & "-" & _month.Value))
            cb_Day.SelectedIndex = 0
            NotifyPropertyChanged("Month")
        End Set
    End Property

    Public Property Year() As DateInfo
        Get
            Return _year
        End Get
        Set(value As DateInfo)
            If _year Is value Then
                Return
            End If
            _year = value
            _date.Day = RangeToDay(DateTime.Parse(_year.Value & "-" & _month.Value))
            cb_Day.SelectedIndex = 0
            NotifyPropertyChanged("Year")
        End Set
    End Property

    Private Function GetDaysInMonth(year As Integer, month As Integer) As Integer
        Return DateTime.DaysInMonth(year, month)
    End Function

    Public Event PropertyChanged As PropertyChangedEventHandler

    Private Sub NotifyPropertyChanged(info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Private Sub DatePicker_Select(sender As Object, e As RoutedEventArgs)
        _iscancel.Cancel = False
        Dim parent As Popup = TryCast(Me.Parent, Popup)
        If parent IsNot Nothing Then
            parent.IsOpen = False
        End If
    End Sub

    Private Sub DatePicker_Cancel(sender As Object, e As RoutedEventArgs)
        _iscancel.Cancel = True
        Dim parent As Popup = TryCast(Me.Parent, Popup)
        If parent IsNot Nothing Then
            parent.IsOpen = False
        End If
    End Sub

    Public Sub ShowDatePicker()
        Dim selectDatePicker As New Popup()
        selectDatePicker.IsLightDismissEnabled = True
        selectDatePicker.Width = Window.Current.Bounds.Width
        selectDatePicker.Height = Window.Current.Bounds.Height
        Me.Height = Window.Current.Bounds.Height
        Me.Width = Window.Current.Bounds.Width
        selectDatePicker.Child = Me
        selectDatePicker.IsOpen = True
        AddHandler selectDatePicker.Closed, Sub(e, s)
                                                RaiseEvent Complete(ToDateTime(), _iscancel)
                                            End Sub
    End Sub

    Public Function ToDateTime() As DateTime
        If _day IsNot Nothing AndAlso _month IsNot Nothing AndAlso _year IsNot Nothing AndAlso _hour IsNot Nothing Then
            Return DateTime.Parse(_year.Value.ToString() & "-" & _month.Value.ToString() & "-" & _day.Value.ToString()).AddHours(_hour.Value)
        End If
        Return DateTime.Now
    End Function

    Private Function GetYearName(year As Integer) As String
        Return DateTimeHelper.ComputeYearGan(New DateTime(year, 1, 1))
    End Function

    Private Function GetHourName(hour As Integer) As String
        Return DateTimeHelper.ComputeTimeGan(DateTimeHelper.ComputeYMDGan(New DateTime(_year.Value, _month.Value, _day.Value)), hour).Substring(6, 2)
    End Function

    Private Function RangeToHour() As ObservableCollection(Of DateInfo)
        Dim data As New ObservableCollection(Of DateInfo)()
        For i As Integer = 0 To 23
            data.Add(New DateInfo() With {
                .Value = i,
                .Info = GetHourName(i)
            })
        Next
        data.Add(data(0))
        '多加一个防止非平板显示不了
        Return data
    End Function

End Class

Public Class DatePickerCompleteEventArgs
    Inherits EventArgs

    Public Property Cancel As Boolean
End Class

Public Class DateInfo
    Public Property Value() As Integer
    Public Property Info() As String
End Class

Public Class [Date]
    Implements INotifyPropertyChanged

    Private _day As ObservableCollection(Of DateInfo)
    Private _month As ObservableCollection(Of DateInfo)
    Private _year As ObservableCollection(Of DateInfo)
    Private _hour As ObservableCollection(Of DateInfo)

    Public Property Day() As ObservableCollection(Of DateInfo)
        Get
            Return _day
        End Get
        Set(value As ObservableCollection(Of DateInfo))
            If _day Is value Then
                Return
            End If
            _day = value
            NotifyPropertyChanged("Day")
        End Set
    End Property

    Public Property Month() As ObservableCollection(Of DateInfo)
        Get
            Return _month
        End Get
        Set(value As ObservableCollection(Of DateInfo))
            If _month Is value Then
                Return
            End If
            _month = value
            NotifyPropertyChanged("Month")
        End Set
    End Property

    Public Property Year() As ObservableCollection(Of DateInfo)
        Get
            Return _year
        End Get
        Set(value As ObservableCollection(Of DateInfo))
            If _year Is value Then
                Return
            End If
            _year = value
            NotifyPropertyChanged("Year")
        End Set
    End Property

    Public Property Hour() As ObservableCollection(Of DateInfo)
        Get
            Return _hour
        End Get
        Set(value As ObservableCollection(Of DateInfo))
            If _hour Is value Then
                Return
            End If
            _hour = value
            NotifyPropertyChanged("Hour")
        End Set
    End Property

    Private Sub NotifyPropertyChanged(info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
