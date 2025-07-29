## Releasing a New Version
To release a new version of your project, use Git tags to mark the commit:
```bash
git tag -a v0.0.1 -m "First release"
git push origin v0.0.1
```

The version number must follow the [Semantic Versioning](https://semver.org/) specification:
* **MAJOR** version: for incompatible API changes
* **MINOR** version: for new features, backward-compatible
* **PATCH** version: for backward-compatible bug fixes

Examples:
* ```v1.0.0``` – First stable release
* ```v1.1.0``` – Adds new functionality without breaking existing code
* ```v1.1.1``` – Fixes a bug

If you need to update an existing tag (e.g., after fixing a mistake), you can delete and re-create it:

```bash
git tag -d v0.0.1
git push origin :refs/tags/v0.0.1
git tag -a v0.0.1 -m "Updated release"
git push origin v0.0.1
```