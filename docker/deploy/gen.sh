if [ -z $BackendIdenty]; then
    BackendIdenty="http://id.mindnote.com"
fi
if [ -z $BackendApi]; then
    BackendApi="http://api.mindnote.com"
fi
if [ -z $FrontendServer]; then
    FrontendServer="http://mindnote.com"
fi
if [ -z $FrontendClient]; then
    FrontendClient="http://client.mindnote.com"
fi

if [ -z $SecretFrontendServer]; then
    SecretFrontendServer="secret"
fi
if [ -z $SecretBackendApi]; then
    SecretBackendApi="secret"
fi
if [ -z $SecretFrontendClient]; then
    SecretFrontendClient="secret"
fi

echo "Encode secrets..."

EncodedSecretFrontendServer=$(echo -n $SecretFrontendServer | openssl dgst -binary -sha256 | base64)
EncodedSecretBackendApi=$(echo -n $SecretBackendApi | openssl dgst -binary -sha256 | base64)
EncodedSecretFrontendClient=$(echo -n $SecretFrontendClient | openssl dgst -binary -sha256 | base64)

echo "  backend-api     : $EncodedSecretBackendApi"
echo "  frontend-server : $EncodedSecretFrontendServer"
echo "  frontend-client : $EncodedSecretFrontendClient"

echo "Copy template files..."

mkdir config
cp -r template/* config/

echo "Apply templates..."

sed -i "s|{BackendIdenty}|$BackendIdenty|g" config/*.json
sed -i "s|{BackendApi}|$BackendApi|g" config/*.json
sed -i "s|{FrontendServer}|$FrontendServer|g" config/*.json
sed -i "s|{FrontendClient}|$FrontendClient|g" config/*.json
sed -i "s|{BackendIdenty}|$(echo $BackendIdenty | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{BackendApi}|$(echo $BackendApi | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{FrontendServer}|$(echo $FrontendServer | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{FrontendClient}|$(echo $FrontendClient | cut -d "/" -f3)|g" config/*.conf
sed -i "s|{SecretFrontendServer}|$SecretFrontendServer|g" config/*
sed -i "s|{SecretBackendApi}|$SecretBackendApi|g" config/*
sed -i "s|{SecretFrontendClient}|$SecretFrontendClient|g" config/*
sed -i "s|{EncodedSecretFrontendServer}|$EncodedSecretFrontendServer|g" config/*
sed -i "s|{EncodedSecretBackendApi}|$EncodedSecretBackendApi|g" config/*
sed -i "s|{EncodedSecretFrontendClient}|$EncodedSecretFrontendClient|g" config/*
