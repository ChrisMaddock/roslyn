﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports System.ComponentModel.Composition
Imports System.Threading
Imports Microsoft.CodeAnalysis.Editor.Implementation.Highlighting
Imports Microsoft.CodeAnalysis.Text
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.Editor.VisualBasic.KeywordHighlighting
    <ExportHighlighter(LanguageNames.VisualBasic)>
    Friend Class EventBlockHighlighter
        Inherits AbstractKeywordHighlighter(Of SyntaxNode)

        <ImportingConstructor>
        Public Sub New()
        End Sub

        Protected Overloads Overrides Sub AddHighlights(node As SyntaxNode, highlights As List(Of TextSpan), cancellationToken As CancellationToken)
            Dim eventBlock = node.GetAncestor(Of EventBlockSyntax)()
            If eventBlock Is Nothing Then
                Return
            End If

            With eventBlock
                With .EventStatement
                    ' This span calculation should also capture the Custom keyword
                    Dim firstKeyword = If(.Modifiers.Count > 0, .Modifiers.First(), .DeclarationKeyword)
                    highlights.Add(TextSpan.FromBounds(firstKeyword.SpanStart, .DeclarationKeyword.Span.End))

                    If .ImplementsClause IsNot Nothing Then
                        highlights.Add(.ImplementsClause.ImplementsKeyword.Span)
                    End If
                End With

                highlights.Add(.EndEventStatement.Span)
            End With
        End Sub
    End Class
End Namespace
