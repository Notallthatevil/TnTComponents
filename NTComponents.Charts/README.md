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
- **Colors & Palettes**: All chart colors must be defined via **CSS variables** sourced from theme files within a specified theme folder (e.g., `LiveTest/LiveTest/wwwroot/Themes/`). The library avoids hardcoded values to ensure full compatibility with your application's theme. For instance, a theme like `light.css` specifies variables on the `:root`:
    ```css
    :root {
        --tnt-color-primary: rgb(84 90 146);
        --tnt-color-secondary: rgb(92 93 114);
    }
    ```
- **Axis Details**: Configure scales, grid lines, tick marks, and crosshairs.
- **Sizing & Responsiveness**: Responsive layouts that adapt to container size with customizable aspect ratios.

## Architectural Principles

### Don't Repeat Yourself (DRY)
The codebase is built on a foundation of reusable abstractions. Shared logic for coordinate systems, legend rendering, and animation loops is centralized in base classes and utility services to ensure consistency and maintainability.

### Declarative Architecture
The library follows a declarative, component-based architecture for building visualizations:

- **NTChart**: The primary container and base class for all charts. It holds all basic rendering and layout information and manages event registration for tracking mouse movement and click events. It houses every chart and provides the core HTML structure and SkiaSharp canvas.
- **Child Content Integration**: `NTChart` allows `ChildContent` to be rendered, enabling a modular approach. These child elements represent the data (series) and decorative components (axes, legends, etc.) to be rendered on the chart.
- **Axes & Scales**: Cartesian charts support declarative axes via `NTCartesianAxis`. Axes are responsible for rendering their own labels and titles, and they automatically adjust the remaining render area for the data series.
- **Series-Based Data Visualization**: Data is represented by series components that inherit from a common hierarchy:
    - **NTBaseSeries**: The abstract base for all data series, representing the data to be rendered. Each series defines its compatible `ChartCoordinateSystem`.
    - **NTCartesianSeries**: A specialized abstraction for cartesian plots using X and Y value selectors.
    - **Concrete Implementations**: Specific series types like `PointSeries`, `LineSeries`, `BarSeries`, etc., provide the final rendering logic.
- **Series Compatibility**: To ensure visual consistency, `NTChart` enforces that all child series share the same `ChartCoordinateSystem`. For example, multiple `LineSeries` (Cartesian) can be combined, but mixing a `LineSeries` with a `PieSeries` (Circular) will result in an exception.

### Built for Testability
The library is designed to be highly testable:
- **Decoupled Logic**: Calculations for layouts, scales, and data processing are separated from the rendering code.
- **Interface-Driven**: Extensive use of interfaces allows for easy mocking of dependencies during unit testing.
- **Reliable Output**: Designed to produce predictable rendering results that can be verified through automated tests.

