Imports System.Globalization

Module MainModule
    Const TianGan As String = "甲乙丙丁戊己庚辛壬癸"
    Const DiZhi As String = "子丑寅卯辰巳午未申酉戌亥"

    Sub Main()
        Dim mydate = New DateTime(1985, 10, 9, 15, 0, 0)
        Console.WriteLine(mydate)
        Console.WriteLine("转换为生辰八字为：")
        Console.WriteLine(EvalBazi(ComputeGan(mydate)))
        Console.Read()
    End Sub

    Function CheckBazi(bazi As String) As Boolean
        Dim baziLen As Integer
        Dim i As Integer

        baziLen = Len(bazi)
        If baziLen <> 6 AndAlso baziLen <> 8 Then Return False

        For i = 0 To baziLen - 1
            If Not (TianGan & DiZhi).Contains(bazi(i)) Then Return False
        Next

        Return True
    End Function

    Function ComputeYMDGan(time As Date) As String
        Return ComputeYearGan(time) & ComputeMonthGan(time) & ComputeDayGan(time)
    End Function

    Function ComputeGan(time As Date) As String
        Return ComputeTimeGan(ComputeYMDGan(time), time.Hour)
    End Function

    '根据出生日子的天干，通过下表来查算时辰干支：
    '时辰干支查算表
    '时间时辰                             五行纪日干支
    '                       甲己     乙庚     丙辛     丁壬     戊癸
    '23－01 子/水           甲子     丙子     戊子     庚子     壬子
    '01－03 丑/土           乙丑     丁丑     己丑     辛丑     癸丑
    '03－05 寅/木           丙寅     戊寅     庚寅     壬寅     甲寅
    '05－07 卯/木           丁卯     己卯     辛卯     癸卯     乙卯
    '07－09 辰/土           戊辰     庚辰     壬辰     甲辰     丙辰
    '09－11 巳/火           己巳     辛巳     癸巳     己巳     丁巳
    '11－13 午/火           庚午     壬午     甲午     丙午     戊午
    '13－15 未/土           辛未     癸未     乙未     丁未     己未
    '15－17 申/金           壬申     甲申     丙申     戊申     庚申
    '17－19 酉/金           癸酉     乙酉     丁酉     己酉     辛酉
    '19－21 戊/土           甲戌     丙戌     戊戌     庚戌     壬戌
    '21－23 亥/水           乙亥     丁亥     己亥     辛亥     癸亥

    Dim cTimeGanZhi_Table(,) As String = {
         {"甲子", "丙子", "戊子", "庚子", "壬子"},
         {"乙丑", "丁丑", "己丑", "辛丑", "癸丑"},
         {"丙寅", "戊寅", "庚寅", "壬寅", "甲寅"},
         {"丁卯", "己卯", "辛卯", "癸卯", "乙卯"},
         {"戊辰", "庚辰", "壬辰", "甲辰", "丙辰"},
         {"己巳", "辛巳", "癸巳", "己巳", "丁巳"},
         {"庚午", "壬午", "甲午", "丙午", "戊午"},
         {"辛未", "癸未", "乙未", "丁未", "己未"},
         {"壬申", "甲申", "丙申", "戊申", "庚申"},
         {"癸酉", "乙酉", "丁酉", "己酉", "辛酉"},
         {"甲戌", "丙戌", "戊戌", "庚戌", "壬戌"},
         {"乙亥", "丁亥", "己亥", "辛亥", "癸亥"}
        }

    Function ComputeYearGan(time As Date) As String
        Dim chineseDate As New ChineseLunisolarCalendar
        Dim Sexagenary = chineseDate.GetSexagenaryYear(time)

        Return TianGan(chineseDate.GetCelestialStem(Sexagenary) - 1) & DiZhi(chineseDate.GetTerrestrialBranch(Sexagenary) - 1)
    End Function

    Function ComputeTimeGan(bazi As String, hour As Integer) As String
        Dim dayGan = bazi(4)

        Dim indexX, indexY As Integer

        Dim i As Integer
        i = TianGan.IndexOf(dayGan)
        If i < 0 Then Return String.Empty
        indexX = i
        If indexX >= 5 Then indexX -= 5
        indexY = (hour + 1) / 2

        Return bazi & cTimeGanZhi_Table(indexY - 1, indexX)
    End Function

    Dim cMonthGanZhi_Table(,) As String = {
        {"丙寅", "丁卯", "戊辰", "己巳", "庚午", "辛未", "壬申", "癸酉", "甲戌", "乙亥", "丙子", "丁丑"},
        {"戊寅", "己卯", "庚辰", "辛巳", "壬午", "癸未", "甲申", "乙酉", "丙戌", "丁亥", "戊子", "己丑"},
        {"庚寅", "辛卯", "壬辰", "癸巳", "甲午", "乙未", "丙申", "丁酉", "戊戌", "己亥", "庚子", "辛丑"},
        {"壬寅", "癸卯", "甲辰", "乙巳", "丙午", "丁未", "戊申", "己酉", "庚戌", "辛亥", "壬子", "癸丑"},
        {"甲寅", "乙卯", "丙辰", "丁巳", "戊午", "己未", "庚申", "辛酉", "壬戌", "癸亥", "甲子", "乙丑"}
        }

    Function ComputeMonthGan(time As Date) As String
        Dim chineseDate As New ChineseLunisolarCalendar
        Dim Sexagenary = chineseDate.GetSexagenaryYear(time)
        Dim SolarTerm() As String = {
            "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏",
            "小满", "芒种", "夏至", "小暑", "大暑", "立秋",
            "处暑", "白露", "秋分", "寒露", "霜降", "立冬",
            "小雪", "大雪", "冬至", "小寒", "大寒", "立春"
            }

        Dim ctfd = getNextSolarTerms(time)
        Dim lMonth As Integer
        For i = 0 To SolarTerm.Length - 1
            If SolarTerm(i) = ctfd Then
                lMonth = i \ 2
                Exit For
            End If
        Next
        'Dim lYear = chineseDate.GetYear(time)
        'Dim lMonth = chineseDate.GetMonth(time)

        ''获取第几个月是闰月,等于0表示本年无闰月
        'Dim leapMonth = chineseDate.GetLeapMonth(lYear)

        ''如果今年有闰月
        'If leapMonth > 0 Then
        '    If lMonth >= leapMonth Then lMonth -= 1
        'End If

        Return cMonthGanZhi_Table((chineseDate.GetCelestialStem(Sexagenary) - 1) Mod 5, lMonth)
    End Function

    Function ComputeDayGan(time As Date) As String
        Dim lyear = time.Year
        Dim lMonth = time.Month
        Dim d = time.Day

        If lMonth = 1 OrElse lMonth = 2 Then
            lMonth += 12
            lyear -= 1
        End If

        Dim C = lyear \ 100
        Dim y = lyear - C * 100
        Dim G = 4 * C + C \ 4 + 5 * y + y \ 4 + 3 * (lMonth + 1) \ 5 + d - 3

        Dim i As Integer = IIf(lMonth Mod 2 = 0, 6, 0)

        Dim Z = 8 * C + C \ 4 + 5 * y + y \ 4 + 3 * (lMonth + 1) \ 5 + d + 7 + i

        Return TianGan((G - 1) Mod 10) & DiZhi((Z - 1) Mod 12)
    End Function

    '十二月份天干强度表
    '生月\四柱天干        甲              乙              丙              丁              戊              己              庚              辛              壬              癸
    '子月                            1.2             1.2             1.0             1.0             1.0             1.0             1.0             1.0             1.2             1.2
    '丑月                            1.06 1.06 1.0             1.0             1.1             1.1             1.14 1.14 1.1             1.1
    '寅月                            1.14 1.14 1.2             1.2             1.06 1.06 1.0             1.0             1.0             1.0
    '卯月                            1.2             1.2             1.2             1.2             1.0             1.0             1.0             1.0             1.0             1.0
    '辰月                            1.1             1.1             1.06 1.06 1.1             1.1             1.1             1.1             1.04 1.04
    '巳月                            1.0             1.0             1.14 1.14 1.14 1.14 1.06 1.06 1.06 1.06
    '午月                            1.0             1.0             1.2             1.2             1.2             1.2             1.0             1.0             1.0             1.0
    '未月                            1.04 1.04 1.1             1.1             1.16 1.16 1.1             1.1             1.0             1.0
    '申月                            1.06 1.06 1.0             1.0             1.0             1.0             1.14 1.14 1.2             1.2
    '酉月                            1.0             1.0             1.0             1.0             1.0             1.0             1.2             1.2             1.2             1.2
    '戌月                            1.0             1.0             1.04 1.04 1.14 1.14 1.16 1.16 1.06 1.06
    '亥月                            1.2             1.2             1.0             1.0             1.0             1.0             1.0             1.0             1.14 1.14

    Dim TianGan_Strength(,) As Double = {
         {1.2, 1.2, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.2, 1.2},
         {1.06, 1.06, 1.0, 1.0, 1.1, 1.1, 1.14, 1.14, 1.1, 1.1},
         {1.14, 1.14, 1.2, 1.2, 1.06, 1.06, 1.0, 1.0, 1.0, 1.0},
         {1.2, 1.2, 1.2, 1.2, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0},
         {1.1, 1.1, 1.06, 1.06, 1.1, 1.1, 1.1, 1.1, 1.04, 1.04},
         {1.0, 1.0, 1.14, 1.14, 1.14, 1.14, 1.06, 1.06, 1.06, 1.06},
         {1.0, 1.0, 1.2, 1.2, 1.2, 1.2, 1.0, 1.0, 1.0, 1.0},
         {1.04, 1.04, 1.1, 1.1, 1.16, 1.16, 1.1, 1.1, 1.0, 1.0},
         {1.06, 1.06, 1.0, 1.0, 1.0, 1.0, 1.14, 1.14, 1.2, 1.2},
         {1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.2, 1.2, 1.2, 1.2},
         {1.0, 1.0, 1.04, 1.04, 1.14, 1.14, 1.16, 1.16, 1.06, 1.06},
         {1.2, 1.2, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.14, 1.14}
        }

    '十二月份地支强度表
    '                        生月       子月         丑月         寅月         卯月         辰月         巳月         午月         未月         申月         酉月         戌月         亥月         
    '地支         支藏
    '子              癸                       1.2             1.1             1.0             1.0             1.04 1.06 1.0             1.0             1.2             1.2             1.06 1.14 
    '丑              癸                       0.36 0.33 0.3             0.3             0.312        0.318        0.3             0.3             0.36 0.36 0.318        0.342
    '丑              辛                       0.2             0.228        0.2             0.2             0.23 0.212        0.2             0.22 0.228        0.248        0.232        0.2            
    '丑              己                       0.5             0.55 0.53 0.5             0.55 0.57 0.6             0.58 0.5             0.5             0.57 0.5             
    '寅              丙                       0.3             0.3             0.36 0.36 0.318        0.342        0.36 0.33 0.3             0.3             0.342        0.318        
    '寅              甲                       0.84 0.742        0.798        0.84 0.77 0.7             0.7             0.728        0.742        0.7             0.7             0.84 
    '卯              乙                       1.2             1.06 1.14 1.2             1.1             1.0             1.0             1.04 1.06 1.0             1.0             1.2            
    '辰              乙                       0.36 0.318        0.342        0.36 0.33 0.3             0.3             0.312        0.318        0.3             0.3             0.36 
    '辰              癸                       0.24 0.22 0.2             0.2             0.208        0.2             0.2             0.2             0.24 0.24 0.212        0.228       
    '辰              戊                       0.5             0.55 0.53 0.5             0.55 0.6             0.6             0.58 0.5             0.5             0.57 0.5             
    '巳              庚                       0.3             0.342        0.3             0.3             0.33 0.3             0.3             0.33 0.342        0.36 0.348        0.3            
    '巳              丙                       0.7             0.7             0.84 0.84 0.742        0.84 0.84 0.798        0.7             0.7             0.728        0.742        
    '午              丁                       1.0             1.0             1.2             1.2             1.06 1.14 1.2             1.1             1.0             1.0             1.04 1.06 
    '未              丁                       0.3             0.3             0.36 0.36 0.318        0.342        0.36 0.33 0.3             0.3             0.312        0.318        
    '未              乙                       0.24 0.212        0.228        0.24 0.22 0.2             0.2             0.208        0.212        0.2             0.2             0.24 
    '未              己                       0.5             0.55 0.53 0.5             0.55 0.57 0.6             0.58 0.5             0.5             0.57 0.5             
    '申              壬                       0.36 0.33 0.3             0.3             0.312        0.318        0.3             0.3             0.36 0.36 0.318        0.342        
    '申              庚                       0.7             0.798        0.7             0.7             0.77 0.742        0.7             0.77 0.798        0.84 0.812        0.7            
    '酉              辛                       1.0             1.14 1.0             1.0             1.1             1.06 1.0             1.1             1.14 1.2             1.16 1.0            
    '戌              辛                       0.3             0.342        0.3             0.3             0.33 0.318        0.3             0.33 0.342        0.36 0.348        0.3             
    '戌              丁                       0.2             0.2             0.24 0.24 0.212        0.228        0.24 0.22 0.2             0.2             0.208        0.212        
    '戌              戊                       0.5             0.55 0.53 0.5             0.55 0.57 0.6             0.58 0.5             0.5             0.57 0.5             
    '亥              甲                       0.36 0.318        0.342        0.36 0.33 0.3             0.3             0.312        0.318        0.3             0.3             0.36 
    '亥              壬                        0.84 0.77 0.7             0.7             0.728        0.742        0.7             0.7             0.84 0.84 0.724        0.798     

    Structure ZISTRENGTH
        Dim diZhi As Char
        Dim zhiCang As Char
        Dim strength() As Double

        Sub New(d As Char, z As Char, s() As Double)
            diZhi = d
            zhiCang = z
            strength = s
        End Sub
    End Structure

    Dim DiZhi_Strength() As ZISTRENGTH = {
         New ZISTRENGTH("子", "癸", {1.2, 1.1, 1.0, 1.0, 1.04, 1.06, 1.0, 1.0, 1.2, 1.2, 1.06, 1.14}),
         New ZISTRENGTH("丑", "癸", {0.36, 0.33, 0.3, 0.3, 0.312, 0.318, 0.3, 0.3, 0.36, 0.36, 0.318, 0.342}),
         New ZISTRENGTH("丑", "辛", {0.2, 0.228, 0.2, 0.2, 0.23, 0.212, 0.2, 0.22, 0.228, 0.248, 0.232, 0.2}),
         New ZISTRENGTH("丑", "己", {0.5, 0.55, 0.53, 0.5, 0.55, 0.57, 0.6, 0.58, 0.5, 0.5, 0.57, 0.5}),
         New ZISTRENGTH("寅", "丙", {0.3, 0.3, 0.36, 0.36, 0.318, 0.342, 0.36, 0.33, 0.3, 0.3, 0.342, 0.318}),
         New ZISTRENGTH("寅", "甲", {0.84, 0.742, 0.798, 0.84, 0.77, 0.7, 0.7, 0.728, 0.742, 0.7, 0.7, 0.84}),
         New ZISTRENGTH("卯", "乙", {1.2, 1.06, 1.14, 1.2, 1.1, 1.0, 1.0, 1.04, 1.06, 1.0, 1.0, 1.2}),
         New ZISTRENGTH("辰", "乙", {0.36, 0.318, 0.342, 0.36, 0.33, 0.3, 0.3, 0.312, 0.318, 0.3, 0.3, 0.36}),
         New ZISTRENGTH("辰", "癸", {0.24, 0.22, 0.2, 0.2, 0.208, 0.2, 0.2, 0.2, 0.24, 0.24, 0.212, 0.228}),
         New ZISTRENGTH("辰", "戊", {0.5, 0.55, 0.53, 0.5, 0.55, 0.6, 0.6, 0.58, 0.5, 0.5, 0.57, 0.5}),
         New ZISTRENGTH("巳", "庚", {0.3, 0.342, 0.3, 0.3, 0.33, 0.3, 0.3, 0.33, 0.342, 0.36, 0.348, 0.3}),
         New ZISTRENGTH("巳", "丙", {0.7, 0.7, 0.84, 0.84, 0.742, 0.84, 0.84, 0.798, 0.7, 0.7, 0.728, 0.742}),
         New ZISTRENGTH("午", "丁", {1.0, 1.0, 1.2, 1.2, 1.06, 1.14, 1.2, 1.1, 1.0, 1.0, 1.04, 1.06}),
         New ZISTRENGTH("未", "丁", {0.3, 0.3, 0.36, 0.36, 0.318, 0.342, 0.36, 0.33, 0.3, 0.3, 0.312, 0.318}),
         New ZISTRENGTH("未", "乙", {0.24, 0.212, 0.228, 0.24, 0.22, 0.2, 0.2, 0.208, 0.212, 0.2, 0.2, 0.24}),
         New ZISTRENGTH("未", "己", {0.5, 0.55, 0.53, 0.5, 0.55, 0.57, 0.6, 0.58, 0.5, 0.5, 0.57, 0.5}),
         New ZISTRENGTH("申", "壬", {0.36, 0.33, 0.3, 0.3, 0.312, 0.318, 0.3, 0.3, 0.36, 0.36, 0.318, 0.342}),
         New ZISTRENGTH("申", "庚", {0.7, 0.798, 0.7, 0.7, 0.77, 0.742, 0.7, 0.77, 0.798, 0.84, 0.812, 0.7}),
         New ZISTRENGTH("酉", "辛", {1.0, 1.14, 1.0, 1.0, 1.1, 1.06, 1.0, 1.1, 1.14, 1.2, 1.16, 1.0}),
         New ZISTRENGTH("戌", "辛", {0.3, 0.342, 0.3, 0.3, 0.33, 0.318, 0.3, 0.33, 0.342, 0.36, 0.348, 0.3}),
         New ZISTRENGTH("戌", "丁", {0.2, 0.2, 0.24, 0.24, 0.212, 0.228, 0.24, 0.22, 0.2, 0.2, 0.208, 0.212}),
         New ZISTRENGTH("戌", "戊", {0.5, 0.55, 0.53, 0.5, 0.55, 0.57, 0.6, 0.58, 0.5, 0.5, 0.57, 0.5}),
         New ZISTRENGTH("亥", "甲", {0.36, 0.318, 0.342, 0.36, 0.33, 0.3, 0.3, 0.312, 0.318, 0.3, 0.3, 0.36}),
         New ZISTRENGTH("亥", "壬", {0.84, 0.77, 0.7, 0.7, 0.728, 0.742, 0.7, 0.7, 0.84, 0.84, 0.724, 0.798})
        }

    '金 --- 0
    '木 --- 1
    '水 --- 2
    '火 --- 3
    '土 --- 4

    Dim WuXingTable() As Char = {"金", "木", "水", "火", "土"}

    '天干地支的五行属性表
    '天干： 甲-木、乙-木、丙-火、丁－火、戊－土、己－土、庚－金、辛－金、壬－水、癸－水 
    '地支：子-水、丑-土、寅-木、卯－木、辰－土、巳－火、午－火、未－土、申－金、酉－金、戌－土、亥－水

    Dim TianGan_WuXingProp() As Integer = {1, 1, 3, 3, 4, 4, 0, 0, 2, 2}
    Dim DiZhi_WuXingProp() As Integer = {2, 4, 1, 1, 4, 3, 3, 4, 0, 0, 4, 2}
    Dim GenerationSourceTable() As Integer = {4, 2, 0, 1, 3}

    Function ComputeGanIndex(gan As Char) As Integer
        Return TianGan.IndexOf(gan)
    End Function

    Function ComputeZhiIndex(zhi As Char) As Integer
        Return DiZhi.IndexOf(zhi)
    End Function

    Function EvalBazi(bazi As String) As String
        Dim result As String = String.Empty

        Dim strengthResult(5) As Double
        Dim monthIndex = ComputeZhiIndex(bazi(3))
        If monthIndex = -1 Then Return String.Empty

        result &= bazi
        result &= vbCrLf & vbCrLf

        For wuXing = 0 To 4
            Dim value1 = 0.0
            Dim value2 = 0.0
            Dim i As Integer
            '扫描4个天干
            For i = 0 To 7 Step 2
                Dim gan = bazi(i)
                Dim index = ComputeGanIndex(gan)
                If index = -1 Then Return String.Empty

                If TianGan_WuXingProp(index) = wuXing Then
                    value1 += TianGan_Strength(monthIndex, index)
                End If
            Next

            '扫描支藏
            For i = 1 To 7 Step 2
                Dim zhi = bazi(i)
                For j = 0 To DiZhi_Strength.Length - 1
                    If DiZhi_Strength(j).diZhi = zhi Then
                        Dim zhiCangIndex = ComputeGanIndex(DiZhi_Strength(j).zhiCang)
                        If zhiCangIndex = -1 Then Return String.Empty
                        If TianGan_WuXingProp(zhiCangIndex) = wuXing Then
                            value2 += DiZhi_Strength(j).strength(monthIndex)
                            Exit For
                        End If
                    End If
                Next
            Next

            strengthResult(wuXing) = value1 + value2

            '输出一行计算结果
            result &= String.Format("{0}:" & vbTab & "{1:N3}+{2:N3}={3:N3}", WuXingTable(wuXing), value1, value2, value1 + value2) & vbCrLf
        Next

        Dim fateProp, srcProp As Integer

        fateProp = TianGan_WuXingProp(ComputeGanIndex(bazi(4)))
        If fateProp = -1 Then Return String.Empty
        result &= vbCrLf & String.Format("命属{0}", WuXingTable(fateProp)) & vbCrLf

        '求同类和异类的强度值
        srcProp = GenerationSourceTable(fateProp)
        Dim tongLei = strengthResult(fateProp) + strengthResult(srcProp)
        Dim yiLei = 0.0
        For i = 0 To 4
            yiLei += strengthResult(i)
        Next
        yiLei -= tongLei

        result &= String.Format("同类：{0}+{1}，", WuXingTable(fateProp), WuXingTable(srcProp)) & String.Format("{0:N3}+{1:N3}={2:N3}", strengthResult(fateProp), strengthResult(srcProp), tongLei) & vbCrLf

        result &= "异类：总和为 " & String.Format("{0:N3}", yiLei) & vbCrLf

        Return result
    End Function

    ''' <summary>
    ''' 定气法计算二十四节气,二十四节气是按地球公转来计算的，并非是阴历计算的
    ''' 节气的定法有两种。古代历法采用的称为"恒气"，即按时间把一年等分为24份，
    ''' 每一节气平均得15天有余，所以又称"平气"。现代农历采用的称为"定气"，即
    ''' 按地球在轨道上的位置为标准，一周360°，两节气之间相隔15°。由于冬至时地
    ''' 球位于近日点附近，运动速度较快，因而太阳在黄道上移动15°的时间不到15天。
    ''' 夏至前后的情况正好相反，太阳在黄道上移动较慢，一个节气达16天之多。采用
    ''' 定气时可以保证春、秋两分必然在昼夜平分的那两天。
    ''' </summary>
    ''' <param name="time">日期</param>
    ''' <returns>节气名称</returns>
    Public Function ChineseTwentyFourDay(time As DateTime) As String
        Dim SolarTerm() As String = {
            "小寒", "大寒", "立春", "雨水", "惊蛰", "春分",
            "清明", "谷雨", "立夏", "小满", "芒种", "夏至",
            "小暑", "大暑", "立秋", "处暑", "白露", "秋分",
            "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"
            }
        Dim sTermInfo() As Integer = {
            0, 21208, 42467, 63836, 85337, 107014,
            128867, 150921, 173149, 195551, 218072, 240693,
            263343, 285989, 308563, 331033, 353350, 375494,
            397447, 419210, 440795, 462224, 483532, 504758
            }
        Dim baseDateAndTime As New DateTime(1900, 1, 6, 2, 5, 0)        '#1/6/1900 2:05:00 AM#
        Dim newDate As DateTime
        Dim num As Double
        Dim y As Integer
        Dim tempStr As String = ""

        y = time.Year

        For i As Integer = 1 To 24
            num = 525948.76 * (y - 1900) + sTermInfo(i - 1)

            newDate = baseDateAndTime.AddMinutes(num)            '按分钟计算
            If newDate.DayOfYear = time.DayOfYear Then
                tempStr = SolarTerm(i - 1)
                Exit For
            End If
        Next
        Return tempStr
    End Function

    ''' <summary>
    ''' 根据当前节气，获取下一个节气的名称及间隔时间
    ''' </summary>
    ''' <param name="beginDate">当前节气的开始时间</param>
    Public Function getNextSolarTerms(beginDate As DateTime) As String
        Dim str As String = ChineseTwentyFourDay(beginDate)
        If str <> "" Then Return str
        For i = 1 To 16
            str = ChineseTwentyFourDay(beginDate.AddDays(i))
            If str <> "" Then Return str
        Next
        Return String.Empty
    End Function
End Module
