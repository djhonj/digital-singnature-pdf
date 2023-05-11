# digital-singnature-pdf
Console application for signing files with .pfk (format PKCS12) extension.

### Technologies used
- db: sql server
- signer: iText7

### Configuration
- To store data in the database, it is necessary to change the connection credentials to your own. To do this you must go to Infrastructure > DBSqlServer > Connection.cs
- The application only accepts certificates with .pfx extension.
- I attach certificate for testing purposes (filename cert_key.pfx).
