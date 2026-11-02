$ErrorActionPreference = 'Stop'

function Convert-ToPdfText([string] $text) {
    $normalized = $text.Normalize([Text.NormalizationForm]::FormD)
    $builder = [Text.StringBuilder]::new()
    foreach ($character in $normalized.ToCharArray()) {
        if ([Globalization.CharUnicodeInfo]::GetUnicodeCategory($character) -ne [Globalization.UnicodeCategory]::NonSpacingMark) {
            [void]$builder.Append($character)
        }
    }

    $plain = $builder.ToString() -replace '\[([^]]+)\]\([^)]*\)', '$1'
    $plain = $plain -replace '`', ''
    $plain = $plain -replace '^\s*[-*]\s+', '• '
    return $plain -split "`r?`n"
}

function Escape-PdfText([string] $line) {
    $ascii = $line -replace '[^\x20-\x7E]', '?'
    return $ascii.Replace('\', '\\').Replace('(', '\(').Replace(')', '\)')
}

function New-SimplePdf([string] $sourcePath, [string] $outputPath, [string] $title) {
    $sourceText = Get-Content $sourcePath -Raw
    $lines = @("$title", "") + @(Convert-ToPdfText $sourceText)
    $linesPerPage = 48
    $pages = @()
    for ($index = 0; $index -lt $lines.Count; $index += $linesPerPage) {
        $end = [Math]::Min($index + $linesPerPage - 1, $lines.Count - 1)
        $pages += ,$lines[$index..$end]
    }

    $objects = [Collections.Generic.List[string]]::new()
    $pageObjectIds = [Collections.Generic.List[int]]::new()
    $fontObjectId = 3 + ($pages.Count * 2)
    $objects.Add('<< /Type /Catalog /Pages 2 0 R >>')
    $objects.Add('')

    $nextObjectId = 3
    foreach ($page in $pages) {
        $pageObjectId = $nextObjectId
        $contentObjectId = $nextObjectId + 1
        $pageObjectIds.Add($pageObjectId)
        $nextObjectId += 2
        $content = "BT`n/F1 10 Tf`n50 760 Td`n"
        foreach ($line in $page) {
            $content += "(" + (Escape-PdfText ([string]$line)) + ") Tj`n0 -14 Td`n"
        }
        $content += 'ET'
        $objects.Add("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 $fontObjectId 0 R >> >> /Contents $contentObjectId 0 R >>")
        $objects.Add("<< /Length $([Text.Encoding]::ASCII.GetByteCount($content)) >>`nstream`n$content`nendstream")
    }

    $kids = ($pageObjectIds | ForEach-Object { "$_ 0 R" }) -join ' '
    $objects[1] = "<< /Type /Pages /Kids [$kids] /Count $($pages.Count) >>"
    $objects.Add('<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>')

    $pdf = [Text.StringBuilder]::new("%PDF-1.4`n")
    $offsets = [Collections.Generic.List[int]]::new()
    for ($objectIndex = 0; $objectIndex -lt $objects.Count; $objectIndex++) {
        $objectId = $objectIndex + 1
        $offsets.Add([Text.Encoding]::ASCII.GetByteCount($pdf.ToString()))
        [void]$pdf.Append("$objectId 0 obj`n$($objects[$objectIndex])`nendobj`n")
    }

    $xrefOffset = [Text.Encoding]::ASCII.GetByteCount($pdf.ToString())
    [void]$pdf.Append("xref`n0 $($objects.Count + 1)`n0000000000 65535 f `n")
    foreach ($offset in $offsets) { [void]$pdf.Append(('{0:0000000000} 00000 n ' -f $offset) + "`n") }
    [void]$pdf.Append("trailer`n<< /Size $($objects.Count + 1) /Root 1 0 R >>`nstartxref`n$xrefOffset`n%%EOF`n")
    [IO.File]::WriteAllBytes($outputPath, [Text.Encoding]::ASCII.GetBytes($pdf.ToString()))
}

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
New-SimplePdf (Join-Path $root 'event-storming.md') (Join-Path $root 'event-storming.pdf') 'Event Storming — Oficina API'
New-SimplePdf (Join-Path $root 'diagramas.md') (Join-Path $root 'diagramas.pdf') 'Diagramas de domínio — Oficina API'
Write-Output 'PDFs gerados com sucesso.'
