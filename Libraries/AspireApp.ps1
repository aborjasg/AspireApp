$url = "https://localhost:7583/getRunImage/1"
$stuff = Invoke-RestMethod -Uri $url -Method Get;
$stuff

Invoke-WebRequest $url -Method Get -Body ($body | ConvertTo-Json) | Select StatusCode, StatusDescription, Content, RawContentLenght

#Invoke-RestMethod -Method 'Post' -Uri $uri  -ContentType 'application/json' -Body ($body | ConvertTo-Json);
Invoke-WebRequest -Uri https://localhost:7583/getSourceData -Method POST -Body $postParams


# API endpoint (header)
$header = @{
 "Accept"="application/json"
 "Content-Type"="application/json"
} 

$testType = "Combined NCP (Miniature)"
$testType = "Combined NCP (Picture)"
$testType = "Spectrum (Picture)"
$testType = "Energy/Electrical (Picture)"
$testType = "Energy Calibration (Picture)"
$testType = "Heatmaps (10x8)"
$testType = "D-Number Stability"

# getSourceData:

$bodySourceData = @{ Name=$testType} | ConvertTo-Json
$responseSourceData = Invoke-RestMethod -Uri "https://localhost:7583/getSourceData" -Method 'Post' -Body $bodySourceData -Headers $header | ConvertTo-Json
$jsonSourceData = $responseSourceData | ConvertFrom-Json
$jsonSourceData

# processData:

$body = @{ "Name"=$testType; "DataSource"=$jsonSourceData.content } | ConvertTo-Json
Invoke-RestMethod -Uri "https://localhost:7583/processData" -Method 'Post' -Body $body -Headers $header | ConvertTo-Json

