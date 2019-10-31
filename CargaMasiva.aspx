<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="CargaMasiva.aspx.cs" Inherits="_IPC2_Proyecto_201612369.Views.CargaMasiva" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
      <%--  <br />
        <asp:FileUpload runat="server" id="Archivo"></asp:FileUpload>--%>
        <br />
        <asp:Button ID="btnCargar" runat="server" CssClass="btn btn-primary" Text="Cargar" OnClick="btnCargar_Click"></asp:Button>
        <br />
        <dx:ASPxFileManager OnFilesUploaded="Archivo_FilesUploaded" Settings-AllowedFileExtensions=".csv" Settings-InitialFolder="C:/" runat="server" ID="Archivo" Theme="Material" >
            <SettingsEditing AllowDelete="True"></SettingsEditing>
            <SettingsEditing AllowDelete="True" AllowRename="True" AllowMove="True" AllowCopy="True"></SettingsEditing>
        </dx:ASPxFileManager>
    </div>
</asp:Content>
