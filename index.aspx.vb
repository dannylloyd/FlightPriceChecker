Imports HtmlAgilityPack
Imports System.Net
Imports System.IO

Public Class index
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            populateAirports()
            txtReturnDate.Attributes.Add("readonly", "readonly")
            txtDepartureDate.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Sub getFares()
        Const taxAndFeePercent As Decimal = 1.1336206897
        Const sept11 As Decimal = 5
        Const segmentFee As Decimal = 3.7 'for one take off and landing
        Const PFCs As Decimal = 18


    End Sub

    Function getWebRequest(ByVal departureDate As Date, _
               ByVal returnDate As Date, _
               ByVal originAirport As String, _
               ByVal destinationAirport As String) As String

        Dim result As String = ""

        Try
            Dim strPost As String = "ss=0&" & _
                                    "fareType=DOLLARS&" & _
                                    "disc=&" & _
                                    "submitButton=&" & _
                                    "previouslySelectedBookingWidgetTab=&" & _
                                    "originAirportButtonClicked=no&" & _
                                    "destinationAirportButtonClicked=no&" & _
                                    "returnAirport=RoundTrip&" & _
                                    "originAirport=" & originAirport & "&" & _
                                    "destinationAirport=" & destinationAirport & "&" & _
                                    "outboundDateString=" & departureDate.ToString("MM/dd/yyyy") & "&" & _
                                    "returnDateString=" & returnDate.ToString("MM/dd/yyyy") & "&" & _
                                    "adultPassengerCount=1&" & _
                                    "seniorPassengerCount=0&" & _
                                    "book_now=Search"

            Dim myWriter As StreamWriter = Nothing
            Dim objRequest As HttpWebRequest = DirectCast(WebRequest.Create("http://www.southwest.com/flight/search-flight.html?int=HOMEQBOMAIR"), HttpWebRequest)
            objRequest.Method = "POST"
            objRequest.ContentLength = strPost.Length
            objRequest.ContentType = "application/x-www-form-urlencoded"
            Try
                myWriter = New StreamWriter(objRequest.GetRequestStream())
                myWriter.Write(strPost)
            Catch c As Exception
                Return "Could not establish connection to server."
            Finally
                myWriter.Close()
            End Try

            Dim objResponse As HttpWebResponse = DirectCast(objRequest.GetResponse(), HttpWebResponse)
            Using sr As New StreamReader(objResponse.GetResponseStream())
                result = sr.ReadToEnd()
                sr.Close()
            End Using

            Return result
        Catch
            Return "Could not establish connection to server"
        End Try
    End Function


    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        litResult.Text = ""
        If validateForm() Then
            Dim southwestResult = getWebRequest(txtDepartureDate.Text,
                              txtReturnDate.Text,
                              ddlOriginAirport.SelectedValue,
                              ddlDestinationAirport.SelectedValue)
            litResult.Text = processResult(southwestResult).Replace("href=""", "href=""http://www.southwest.com")
        Else
            litResult.Text = "Please enter all fields"
        End If
    End Sub

    Function convertNodesToUniqueLists(nodes As HtmlAgilityPack.HtmlNodeCollection, type As String) As String
        Dim list As New ListItemCollection
        For Each node In nodes
            list.Add(String.Format("{0}", node.InnerHtml.Replace("<span class=""currency_symbol"">$</span>", "").Replace(" ", "").TrimStart.TrimEnd.Trim))
        Next

        Dim result As String = String.Format("<h1>{0}</h1>", type)
        result += "<ul class=""prices"" id=""" & type & """>"
        Dim nonRepeats = (From n In list).Distinct().ToArray
        For Each asdf In nonRepeats
            result += String.Format("<li>${0}</li>", asdf)
        Next
        result += "</ul>"
        Return result
    End Function

    Function processResult(result As String) As String
        Dim doc As New HtmlAgilityPack.HtmlDocument
        doc.LoadHtml(result)
        Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//*[@id=""faresOutbound""]//label[@class='product_price']")
        Dim nodes2 As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//*[@id=""faresReturn""]//label[@class='product_price']")

        processResult = ""
        If nodes IsNot Nothing Then
            processResult += convertNodesToUniqueLists(nodes, "DepartureFlight")
            processResult += convertNodesToUniqueLists(nodes2, "ReturnFlight")
        Else
            Dim errorNode As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//div[@id='error_wrapper']")
            If errorNode IsNot Nothing Then
                Return errorNode.InnerHtml
            Else
                Return "Nothing matched xpath"
            End If
        End If
    End Function

    Private Sub btnGoToFare_Click(sender As Object, e As System.EventArgs) Handles btnGoToFare.Click
        litResult.Text = ""
        If validateForm() Then
            litResult.Text = getWebRequest(txtDepartureDate.Text,
                              txtReturnDate.Text,
                              ddlOriginAirport.SelectedValue,
                              ddlDestinationAirport.SelectedValue).Replace("/flight/", "http://www.southwest.com/flight/")
        Else
            litResult.Text = "Please enter all fields"

        End If
        
    End Sub

    Function validateForm() As Boolean
        Return IsDate(txtDepartureDate.Text) AndAlso
            IsDate(txtReturnDate.Text) AndAlso
            ddlDestinationAirport.SelectedIndex > 0 AndAlso
            ddlDestinationAirport.SelectedIndex > 0
    End Function

    Sub populateAirports()
        Dim airports As New DropDownList
        airports.Items.Add(New ListItem("Acapulco, MX - ACA", "ACA"))
        airports.Items.Add(New ListItem("Aguascalientes, MX - AGU", "AGU"))
        airports.Items.Add(New ListItem("Albany, NY - ALB", "ALB"))
        airports.Items.Add(New ListItem("Albuquerque, NM - ABQ", "ABQ"))
        airports.Items.Add(New ListItem("Amarillo, TX - AMA", "AMA"))
        airports.Items.Add(New ListItem("Austin, TX - AUS", "AUS"))
        airports.Items.Add(New ListItem("Baltimore/Washington, MD - BWI", "BWI"))
        airports.Items.Add(New ListItem("Birmingham, AL - BHM", "BHM"))
        airports.Items.Add(New ListItem("Boise, ID - BOI", "BOI"))
        airports.Items.Add(New ListItem("Boston Logan, MA - BOS", "BOS"))
        airports.Items.Add(New ListItem("Manchester, NH - MHT", "MHT"))
        airports.Items.Add(New ListItem("Providence, RI - PVD", "PVD"))
        airports.Items.Add(New ListItem("Boston Logan, MA - BOS", "BOS"))
        airports.Items.Add(New ListItem("Buffalo/Niagara, NY - BUF", "BUF"))
        airports.Items.Add(New ListItem("Burbank, CA - BUR", "BUR"))
        airports.Items.Add(New ListItem("Cancun, MX - CUN", "CUN"))
        airports.Items.Add(New ListItem("Charleston, SC - CHS", "CHS"))
        airports.Items.Add(New ListItem("Chicago (Midway), IL - MDW", "MDW"))
        airports.Items.Add(New ListItem("Chihuahua, MX - CUU", "CUU"))
        airports.Items.Add(New ListItem("Cleveland, OH - CLE", "CLE"))
        airports.Items.Add(New ListItem("Columbus, OH - CMH", "CMH"))
        airports.Items.Add(New ListItem("Corpus Christi, TX - CRP", "CRP"))
        airports.Items.Add(New ListItem("Cuernavaca, MX - CVJ", "CVJ"))
        airports.Items.Add(New ListItem("Culiacan, MX - CUL", "CUL"))
        airports.Items.Add(New ListItem("Dallas (Love Field), TX - DAL", "DAL"))
        airports.Items.Add(New ListItem("Denver, CO - DEN", "DEN"))
        airports.Items.Add(New ListItem("Detroit, MI - DTW", "DTW"))
        airports.Items.Add(New ListItem("El Paso, TX - ELP", "ELP"))
        airports.Items.Add(New ListItem("Ft. Lauderdale, FL - FLL", "FLL"))
        airports.Items.Add(New ListItem("Ft. Myers, FL - RSW", "RSW"))
        airports.Items.Add(New ListItem("Greenville/Spartanburg, SC - GSP", "GSP"))
        airports.Items.Add(New ListItem("Guadalajara, MX - GDL", "GDL"))
        airports.Items.Add(New ListItem("Harlingen, TX - HRL", "HRL"))
        airports.Items.Add(New ListItem("Hartford, CT - BDL", "BDL"))
        airports.Items.Add(New ListItem("Hermosillo, MX - HMO", "HMO"))
        airports.Items.Add(New ListItem("Houston (Hobby), TX - HOU", "HOU"))
        airports.Items.Add(New ListItem("Indianapolis, IN - IND", "IND"))
        airports.Items.Add(New ListItem("Jackson, MS - JAN", "JAN"))
        airports.Items.Add(New ListItem("Jacksonville, FL - JAX", "JAX"))
        airports.Items.Add(New ListItem("Kansas City, MO - MCI", "MCI"))
        airports.Items.Add(New ListItem("La Paz, MX - LAP", "LAP"))
        airports.Items.Add(New ListItem("Las Vegas, NV - LAS", "LAS"))
        airports.Items.Add(New ListItem("Leon Bajio, MX - BJX", "BJX"))
        airports.Items.Add(New ListItem("Little Rock, AR - LIT", "LIT"))
        airports.Items.Add(New ListItem("Long Island, NY - ISP", "ISP"))
        airports.Items.Add(New ListItem("Los Angeles, CA - LAX", "LAX"))
        airports.Items.Add(New ListItem("Burbank, CA - BUR", "BUR"))
        airports.Items.Add(New ListItem("Los Angeles, CA - LAX", "LAX"))
        airports.Items.Add(New ListItem("Ontario/LA, CA - ONT", "ONT"))
        airports.Items.Add(New ListItem("Orange County, CA - SNA", "SNA"))
        airports.Items.Add(New ListItem("Los Cabos, MX - SJD", "SJD"))
        airports.Items.Add(New ListItem("Los Mochis, MX - LMM", "LMM"))
        airports.Items.Add(New ListItem("Louisville, KY - SDF", "SDF"))
        airports.Items.Add(New ListItem("Lubbock, TX - LBB", "LBB"))
        airports.Items.Add(New ListItem("Manchester, NH - MHT", "MHT"))
        airports.Items.Add(New ListItem("Mazatlan, MX - MZT", "MZT"))
        airports.Items.Add(New ListItem("Mexicali, MX - MXL", "MXL"))
        airports.Items.Add(New ListItem("Mexico City/D.F., MX - MEX", "MEX"))
        airports.Items.Add(New ListItem("Mexico City/Toluca, MX - TLC", "TLC"))
        airports.Items.Add(New ListItem("Ft. Lauderdale, FL - FLL", "FLL"))
        airports.Items.Add(New ListItem("Midland/Odessa, TX - MAF", "MAF"))
        airports.Items.Add(New ListItem("Milwaukee, WI - MKE", "MKE"))
        airports.Items.Add(New ListItem("Minneapolis/St. Paul, MN - MSP", "MSP"))
        airports.Items.Add(New ListItem("Monterrey, MX - MTY", "MTY"))
        airports.Items.Add(New ListItem("Morelia, MX - MLM", "MLM"))
        airports.Items.Add(New ListItem("Nashville, TN - BNA", "BNA"))
        airports.Items.Add(New ListItem("New Orleans, LA - MSY", "MSY"))
        airports.Items.Add(New ListItem("New York (LaGuardia), NY - LGA", "LGA"))
        airports.Items.Add(New ListItem("Long Island, NY - ISP", "ISP"))
        airports.Items.Add(New ListItem("New York (LaGuardia), NY - LGA", "LGA"))
        airports.Items.Add(New ListItem("Newark, NJ - EWR", "EWR"))
        airports.Items.Add(New ListItem("Newark, NJ - EWR", "EWR"))
        airports.Items.Add(New ListItem("Norfolk, VA - ORF", "ORF"))
        airports.Items.Add(New ListItem("Panama City Beach, FL - ECP", "ECP"))
        airports.Items.Add(New ListItem("Oakland, CA - OAK", "OAK"))
        airports.Items.Add(New ListItem("Oaxaca, MX - OAX", "OAX"))
        airports.Items.Add(New ListItem("Oklahoma City, OK - OKC", "OKC"))
        airports.Items.Add(New ListItem("Omaha, NE - OMA", "OMA"))
        airports.Items.Add(New ListItem("Ontario/LA, CA - ONT", "ONT"))
        airports.Items.Add(New ListItem("Orange County, CA - SNA", "SNA"))
        airports.Items.Add(New ListItem("Orlando, FL - MCO", "MCO"))
        airports.Items.Add(New ListItem("Panama City Beach, FL - ECP", "ECP"))
        airports.Items.Add(New ListItem("Philadelphia, PA - PHL", "PHL"))
        airports.Items.Add(New ListItem("Phoenix, AZ - PHX", "PHX"))
        airports.Items.Add(New ListItem("Pittsburgh, PA - PIT", "PIT"))
        airports.Items.Add(New ListItem("Portland, OR - PDX", "PDX"))
        airports.Items.Add(New ListItem("Providence, RI - PVD", "PVD"))
        airports.Items.Add(New ListItem("Puebla, MX - PBC", "PBC"))
        airports.Items.Add(New ListItem("Puerto Vallarta, MX - PVR", "PVR"))
        airports.Items.Add(New ListItem("Raleigh/Durham, NC - RDU", "RDU"))
        airports.Items.Add(New ListItem("Reno/Tahoe, NV - RNO", "RNO"))
        airports.Items.Add(New ListItem("Sacramento, CA - SMF", "SMF"))
        airports.Items.Add(New ListItem("Salt Lake City, UT - SLC", "SLC"))
        airports.Items.Add(New ListItem("San Antonio, TX - SAT", "SAT"))
        airports.Items.Add(New ListItem("San Diego, CA - SAN", "SAN"))
        airports.Items.Add(New ListItem("San Francisco, CA - SFO", "SFO"))
        airports.Items.Add(New ListItem("Oakland, CA - OAK", "OAK"))
        airports.Items.Add(New ListItem("San Jose, CA - SJC", "SJC"))
        airports.Items.Add(New ListItem("San Francisco, CA - SFO", "SFO"))
        airports.Items.Add(New ListItem("San Jose, CA - SJC", "SJC"))
        airports.Items.Add(New ListItem("Seattle/Tacoma, WA - SEA", "SEA"))
        airports.Items.Add(New ListItem("Spokane, WA - GEG", "GEG"))
        airports.Items.Add(New ListItem("St. Louis, MO - STL", "STL"))
        airports.Items.Add(New ListItem("Tampa Bay, FL - TPA", "TPA"))
        airports.Items.Add(New ListItem("Tijuana, MX - TIJ", "TIJ"))
        airports.Items.Add(New ListItem("Tucson, AZ - TUS", "TUS"))
        airports.Items.Add(New ListItem("Tulsa, OK - TUL", "TUL"))
        airports.Items.Add(New ListItem("Uruapan, MX - UPN", "UPN"))
        airports.Items.Add(New ListItem("Washington (Dulles), DC - IAD", "IAD"))
        airports.Items.Add(New ListItem("Baltimore/Washington, MD - BWI", "BWI"))
        airports.Items.Add(New ListItem("Washington (Dulles), DC - IAD", "IAD"))
        airports.Items.Add(New ListItem("West Palm Beach, FL - PBI", "PBI"))
        airports.Items.Add(New ListItem("Zacatecas, MX - ZCL", "ZCL"))

        ddlOriginAirport.Items.Clear()
        ddlDestinationAirport.Items.Clear()

        ddlOriginAirport.Items.Add(New ListItem("Select a From Airport", ""))
        ddlDestinationAirport.Items.Add(New ListItem("Select a To Airport", ""))
        For Each item As ListItem In airports.Items
            ddlOriginAirport.Items.Add(item)
            ddlDestinationAirport.Items.Add(item)
        Next
    End Sub
End Class
