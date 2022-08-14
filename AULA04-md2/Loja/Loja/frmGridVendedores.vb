﻿Imports System.Data.SqlClient

Public Class frmGridVendedores

    Private con As New SqlConnection("Initial catalog=ExMercado;server=L3CORE517\SQLEXPRESS;UID=sa;PWD=etec")
    Private dtb As New DataTable()

    Private Sub frmGridClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AtualizaGrid()
        gridVendedores.DataSource = dtb
    End Sub

    Private Sub AtualizaGrid()
        con.Open()
        Dim adp As New SqlDataAdapter("Select codigo AS [Código], nome AS [Nome], percentualComissao AS [Percentual] from Vendedores", con)
        dtb.Clear()
        adp.Fill(dtb)
        con.Close()
    End Sub

    Private Sub btnInserir_Click(sender As Object, e As EventArgs) Handles btnInserir.Click
        frmDetalhesVendedores.txtCodigo.Text = "(novo)"
        frmDetalhesVendedores.txtNome.Text = ""
        frmDetalhesVendedores.txtPercentual.Text = ""
        frmDetalhesVendedores.ShowDialog()

        If (frmDetalhesVendedores.DialogResult = Windows.Forms.DialogResult.OK) Then
            con.Open()
            Dim cmd As New SqlCommand("INSERT INTO Vendedores VALUES ('" &
                                      frmDetalhesVendedores.txtNome.Text & "', '" &
                                      frmDetalhesVendedores.txtPercentual.Text & "')", con)
            cmd.ExecuteNonQuery()

            con.Close()
            AtualizaGrid()
        End If
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        frmDetalhesVendedores.txtCodigo.Text = dtb.Rows(gridVendedores.CurrentRow.Index).Item("Código")
        frmDetalhesVendedores.txtNome.Text = dtb.Rows(gridVendedores.CurrentRow.Index).Item("Nome")
        frmDetalhesVendedores.txtPercentual.Text = dtb.Rows(gridVendedores.CurrentRow.Index).Item("Percentual")
        frmDetalhesVendedores.ShowDialog()

        If (frmDetalhesVendedores.DialogResult = Windows.Forms.DialogResult.OK) Then
            con.Open()
            Dim cmd As New SqlCommand("UPDATE Vendedores SET nome = '" &
                                      frmDetalhesVendedores.txtNome.Text & "', percentualComissao = '" &
                                      frmDetalhesVendedores.txtPercentual.Text &
                                      "' WHERE codigo = " & frmDetalhesVendedores.txtCodigo.Text, con)
            cmd.ExecuteNonQuery()
            con.Close()
            AtualizaGrid()
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If MsgBox("Deseja realmente excluir?", MsgBoxStyle.YesNo, "Excluir") = MsgBoxResult.Yes Then
            con.Open()
            Dim cmd As New SqlCommand("DELETE FROM Vendedores " &
                                      "WHERE codigo = " & dtb.Rows(gridVendedores.CurrentRow.Index).Item("Código"), con)
            cmd.ExecuteNonQuery()
            con.Close()
            AtualizaGrid()

        End If
    End Sub
End Class