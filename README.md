# Playwright C# Test Automation with CI/CD Pipeline

This repository contains **end-to-end test automation** scripts using **Playwright** for **C#**. The setup is designed for seamless integration with **GitHub Actions** for continuous integration and continuous deployment (CI/CD) pipelines.

## 🚀 Getting Started

Follow these steps to get the test automation suite running both locally and in a CI/CD pipeline:

### Prerequisites

- **.NET SDK** (version 8.0 or higher) installed
- **Playwright** installed globally (`dotnet tool install --global Microsoft.Playwright.CLI`)
- **GitHub Repository Secrets** set for sensitive data (like username and password)

---

## 🛠️ Setting Up Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/repo-name.git
   cd repo-name
   ```

2. Install .NET dependencies:
   ```bash
   dotnet restore
   ```

3. Install Playwright dependencies:
   ```bash
   dotnet tool install --global Microsoft.Playwright.CLI
   playwright install
   ```

4. Run tests locally:
   ```bash
   dotnet test
   ```

---

## 🔧 CI/CD Setup with GitHub Actions

### Overview

We have set up a **GitHub Actions** pipeline to run Playwright tests automatically whenever code is pushed or a pull request is created. The pipeline ensures that the code is tested in a **clean, isolated environment** and reports the test results, artifacts, and logs.

### Key Features:
- **Secrets management**: Secure storage and passing of sensitive data like login credentials.
- **Headless testing**: Run tests in a browser without a graphical interface for faster execution.
- **Automated Test Reports**: Capture and store test results and videos/screenshots for debugging.

### CI/CD Pipeline

The pipeline is defined in `.github/workflows/playwright.yml`. It includes steps to:
- **Checkout the code**.
- **Install .NET SDK and Playwright**.
- **Run tests** on every push to the `main` branch or when a pull request is made.

Here’s an example of the GitHub Actions workflow configuration:

```yaml
name: Run Playwright Tests

on:
  push:
    branches: [ main ]
  pull_request:

jobs:
  test:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x' # or your version

      - name: Install dependencies
        run: dotnet restore

      - name: Run Playwright install
        run: |
          dotnet tool install --global Microsoft.Playwright.CLI
          playwright install

      - name: Run Tests
        env:
          LOGIN_USERNAME: ${{ secrets.LOGIN_USERNAME }}
          LOGIN_PASSWORD: ${{ secrets.LOGIN_PASSWORD }}
        run: dotnet test
```

---

## 🔒 Secrets and Environment Variables

For secure handling of sensitive information such as login credentials, **GitHub Secrets** are used in the CI/CD pipeline. The following secrets should be configured in your GitHub repository under **Settings → Secrets and variables → Actions**:

- `LOGIN_USERNAME` – Your username for logging into the application.
- `LOGIN_PASSWORD` – Your password for logging into the application.

These secrets are passed securely to the test code during the execution of GitHub Actions.

### Accessing Secrets in C# Code

In your Playwright test scripts, you can access these secrets using the following method:

```csharp
var username = Environment.GetEnvironmentVariable("LOGIN_USERNAME");
var password = Environment.GetEnvironmentVariable("LOGIN_PASSWORD");

await page.FillAsync("#username", username);
await page.FillAsync("#password", password);
await page.ClickAsync("button[type=submit]");
```

---

## 🐞 Debugging and Reporting

### Capturing Screenshots and Videos

To aid in debugging, **Playwright** supports capturing **screenshots** and **videos** during the test execution. You can configure this in your test setup:

```csharp
await page.ScreenshotAsync(new ScreenshotOptions { Path = "screenshot.png" });
await context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true });
```

### Test Artifacts

In case of test failure, **Playwright** will capture the necessary artifacts like screenshots, videos, and logs. These can be used for debugging purposes.

---

## ❌ Common Issues & Troubleshooting

### 1. **Playwright Not Installed**
- Ensure the `playwright install` command is executed in your GitHub Actions pipeline.
- Run `dotnet tool install --global Microsoft.Playwright.CLI` if you haven't installed Playwright globally.

### 2. **Missing Environment Variables or Secrets**
- Double-check that all secrets (`LOGIN_USERNAME`, `LOGIN_PASSWORD`) are configured in GitHub Secrets.

### 3. **Test Flakiness / Timing Issues**
- Use Playwright’s **auto-waiting** features, such as `await page.WaitForSelector(...)` to handle slow-loading elements.

### 4. **Incorrect Base URLs**
- Ensure that URLs (like for staging or production environments) are correctly configured in the CI pipeline and as environment variables.

---

## ⚙️ Playwright Test Configuration

You can modify the **Playwright test** settings in the `playwright.config.ts` file (for JavaScript/TypeScript users) or within your C# test setup. Some useful configurations include:
- Test retries
- Timeouts
- Browser settings (e.g., headless mode)

---

## 👨‍💻 Contributing

Feel free to open issues or submit pull requests to improve the automation suite or address bugs. Contributions are welcome!

1. Fork the repository.
2. Create a new branch.
3. Make your changes and commit.
4. Push your changes to your fork.
5. Submit a pull request for review.

---

## 📚 Resources

- [Playwright for C# Documentation](https://playwright.dev/dotnet)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Playwright C# GitHub Repository](https://github.com/microsoft/playwright-dotnet)

---

## 📝 License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

---

### 🎉 Happy testing!

---

This **README.md** template covers the setup for both local and CI environments, GitHub Actions pipeline, common pitfalls, and troubleshooting. It provides clear steps for users to get started with your Playwright-based automation testing suite while highlighting best practices.

Would you like to further customize this template or add more specific sections? Let me know!
