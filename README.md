![logo](https://raw.githubusercontent.com/williamniemiec/MetricsIntegrator/master/docs/img/logo/logo.png?raw=true)

<h1 align='center'>Metrics Integrator</h1>
<p align='center'>A simple tool that uses as input the source code metrics file and the mapping between application methods and the test methods. As output, our application yields a file where each instance involves source code metrics of one test method and the particular application method under test.</p>
<p align="center">
	<a href="https://github.com/williamniemiec/MetricsIntegrator/actions?query=workflow%3AWindows"><img src="https://img.shields.io/github/workflow/status/williamniemiec/MetricsIntegrator/Windows?label=Windows" alt=""></a>
	<a href="https://github.com/williamniemiec/MetricsIntegrator/actions?query=workflow%3AMacOS"><img src="https://img.shields.io/github/workflow/status/williamniemiec/MetricsIntegrator/MacOS?label=MacOS" alt=""></a>
	<a href="https://github.com/williamniemiec/MetricsIntegrator/actions?query=workflow%3AUbuntu"><img src="https://img.shields.io/github/workflow/status/williamniemiec/MetricsIntegrator/Ubuntu?label=Ubuntu" alt=""></a>
	<a href="https://codecov.io/gh/williamniemiec/MetricsIntegrator"><img src="https://codecov.io/gh/williamniemiec/MetricsIntegrator/branch/master/graph/badge.svg?token=R2SFS4SP86" alt="Coverage status"></a>
	<a href="http://java.oracle.com"><img src="https://img.shields.io/badge/.NET-5.0+-D0008F.svg" alt="Dotnet compatibility"></a>
	<a href="https://github.com/williamniemiec/MetricsIntegrator/releases"><img src="https://img.shields.io/github/v/release/williamniemiec/MetricsIntegrator" alt="Release"></a>
</p>
<hr />

## ❇ Introduction
A simple tool that uses as input the source code metrics file and the mapping between application methods and the test methods. As output, our application yields a file where each instance involves source code metrics of one test method and the particular application method under test.

We developed this tool to generate the defined metrics to build the dataset detailed in the paper "A dataset based on two graph coverage criteria: prime-path and edge coverage"

## ⚠ Requirements
- CSV separated by ';'
- First column of source code metrics, test code metrics and test path metrics must be the method signature 

## Gallery

#### Home
![home-img](https://raw.githubusercontent.com/williamniemiec/MetricsIntegrator/master/docs/img/screens/home.png?raw=true)

#### Export selection
![export-gif](https://raw.githubusercontent.com/williamniemiec/MetricsIntegrator/master/docs/gif/screens/export.gif?raw=true)

## 📁 Files
### /
|        Name 	|Type|Description|
|----------------|-------------------------------|-----------------------------|
|dist |`Directory`|Released versions|
|docs |`Directory`|Documentation files|
|src     |`Directory`| Source files |
|test     |`Directory`| Test files |
