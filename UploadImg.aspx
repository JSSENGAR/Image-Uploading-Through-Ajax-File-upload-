<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadImages.aspx.cs" Inherits="NPage.Upload/Img" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Students : photo uploader </h1>
    <p style="color: #FF0000">(Photo name must be scholar id like AXXXXX.jpg)</p>
    
           <ajaxToolkit:AjaxFileUpload ID="AjaxFileUpload11" runat="server"  OnUploadComplete="OnUploadComplete" />
                
</asp:Content>
