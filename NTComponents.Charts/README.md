# NTComponents.Charts

A high-performance, animated charting library for Blazor powered by SkiaSharp.

## Overview

NTComponents.Charts is a comprehensive data visualization library built for Blazor applications. Leveraging the power of the **SkiaSharp** rendering engine, it providing a rich set of features to create beautiful, interactive, and high-performance charts.

## Key Features

### Extensive Chart Gallery
The library aims to support a wide range of visualization types, including but not limited to:
- **Line & Area Charts**: Perfect for visualizing trends over time.
- **Bar & Column Charts**: Ideal for categorical comparisons.
- **Pie & Doughnut Charts**: Show part-to-whole relationships effectively.
- **Scatter & Bubble Plots**: Explore correlations and multi-dimensional data points.
- **Radar & Polar Area Charts**: Compare multivariate data across several axes.
- **Map Plots**: Visualize geographic distribution and spatial data.
- **Treemaps & Heatmaps**: Understand hierarchical structures and data density.
- **Gauges & Indicators**: Monitor KPIs and progress against targets.
- **Advanced Types**: Support for Waterfall charts, Box Plots, and more.

### Fluid Animations
Every chart supports configurable animations for initial loading and data updates, ensuring a polished and engaging user experience.

### Rich Interactivity
Charts are designed to be fully interactive with seamless, animated responses to user input:
- **Hit-Testing & Selection**: Accurately detect clicks or taps on individual data points, segments, or series.
- **Hover Effects & Tooltips**: Dynamic, animated tooltips and highlight states as users move their cursor across the data.
- **Zooming & Panning**: Smoothly navigate through large datasets with animated transitions between different zoom levels and perspectives.
- **Interactive Legends**: Toggle visibility of data series with elegant animation transitions.

### Granular Customization
Full control over the look and feel of your visualizations:
- **Labels & Titles**: Customize fonts, positioning, and content for all text elements.
- **Colors & Palettes**: Colors are configured using **CSS variable names** for seamless theme integration. The library avoids predefined palettes to ensure full compatibility with your application's design system.
- **Axis Details**: Configure scales, grid lines, tick marks, and crosshairs.
- **Sizing & Responsiveness**: Responsive layouts that adapt to container size with customizable aspect ratios.

## Architectural Principles

### Don't Repeat Yourself (DRY)
The codebase is built on a foundation of reusable abstractions. Shared logic for coordinate systems, legend rendering, and animation loops is centralized in base classes and utility services to ensure consistency and maintainability.

### Encapsulation & Inheritance
Following clean code principles, the library utilizes:
- **Encapsulation**: Component state and rendering internals are managed internally, exposing only necessary parameters to the user.
- **Inheritance**: Specific chart types inherit from generalized base components (e.g., `CartesianChartBase`, `PolarChartBase`), allowing for rapid development of new types while maintaining a unified API.

### Built for Testability
The library is designed to be highly testable:
- **Decoupled Logic**: Calculations for layouts, scales, and data processing are separated from the rendering code.
- **Interface-Driven**: Extensive use of interfaces allows for easy mocking of dependencies during unit testing.
- **Reliable Output**: Designed to produce predictable rendering results that can be verified through automated tests.

