#! /usr/bin/env sh

###################################################################################
#                             REQUEST CUSTOMER ACCESS TOKEN                       #
###################################################################################
# This script requests customer access token based on authorization code using-   #
# the endpoint "oauth2/token". You must request an application access token to 	  #
# run this script. Please update the variables "accessToken", "certPath" and      #
# "authorization_code"      													  #
###################################################################################

keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
certPath="/certs/"  # path of the downloaded certificates and keys
authorization_code="8b6cd77a-aa44-4527-ab08-a58d70cca286" # generated value of authorization code from the previous step.

# Generated value of application access token. Please note that the access token expires in 15 minutes

accessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..DrCjx3IURF7XhEtry-Ln1Q.k45FohB2Iy3shVlx8-cMHGSGBl3VQeZK3wNnhsqipCVnazVx6dT6A5QQBLQDuHJwBD41BSYQT-y4ugf1qrZDBZAF7s5NTMHINgOl13S2xXX7Qy52Zfk4cK6BM31kpz9akuGrGGsuUUEADP84-fq9dZiesZ7GhlXBDnZ5hnGgq304w8mu46ytxnkyH-IJ4q0RKTJRN8VUngDUB0g1qG8Q-CKwXdmeSNGfWQwR2aUEiZLW6oklEz66pWyOS2401uX40SRBCY02EaoqQfJrBlvLmlVI6hypmxB5z8CCdt7FU2ctBOxc6k8gTPqJszmxZcfrCuQDUo9tgwxHtSaOiFVp99LjjA68VBD7qxKgo2u2i3ubT3uZuLmcFksoH4oLP_9X9DqnJg0FK2Q7aPCO7WAWrVTaz8PigZjJK8x-GA2q5BbUcXU95cw9T0JHTZ9OhEaI-r9BSiGyB0DOaVTav0cn6SYY5OToAeTnbt5dkKG6Wf3e8k5ztcwA0AouUvdZG_pTeWnP7hOIcDUcecnFyPvZeaFjG3Q0SDEVahwcx9TlVdSr0b5qPf9p5-gfSgu-4Sc4cZtIbfsuuzpEfAlR_6dc2omWC4JwqwYFxC2tq_gM92lsl_aRB2Ya2tqhSSB2OEJeukmctixHmLyk8ItyvzvlE-AUfnOasyJGVXTG9simysdetdX0oyK6FICC3qSk1QP2FQGB9nc6IZFD8fYoh7wbiTcf3NiJQXoU2GTKrmqXCyMepDYVt3nY6buGpoEUm_2JKZQY3I1hRHam0BfwV_aZFqBBTwrQFTVJoJLaIU7OxKXdtogl073AtfkdyA2w4IoHDN4Bitub_mL-AqXJ2NCjpWTSxTaF4fNfuKn6bCX43PCVP2_1-I0E3La9ppkoLbKl2xgjvCEtbBYaNGRLewJ9t8yemRh9uTGTb7iSp0PF4M9JUymJDKNUM4AdlwnfF2X2HgWJ2eWWq9m1IFKWa5OuKfCaxPreP9aT_OSPk00XudHmqvTOHAiZOiVsl8ww8cbb08Ky76UhWbO302z8IM15-0oPDR2OCVsZwAganocm11jSZtL0WAN5SC5L88thqrzNiexh-fUE8hsn1fiGSgdkHyGhNEFBLNf6_oX0i1Uzuuc79Dk8qZji28uUPx1fuyZRjBRKAQSTkSYfOP-6GtjMgVu8BRTPJ8vOWzbfam9Dvax20ZGIcwh8VkuyzTPrOQu2whKdEu5BzTFuLGOWo2ij4BCNdZmzKb6zmo53-kwEWkVdHVqKrXfcPpMrbR2weiJhBgC198OJ_80O9TOSLrHfZ-HgHHsz37_GIqS62lnfqjPKaSNsvfZ-zucxaXdj5WGrYI1hxUsOdpurhGut1TexaK3IKg07riaIjNOiNWUYDxim54OM1RnTfwbL9jEFmYv2y7GCW7fI14xga_bh2x0aC5rkRF7ZwjxtJZPbT7IcLbuj8jf8dXbMp9HPVm1QGQidYSzNVwZ6D7Vyn3fp2x1DjMqiLV5Hiu229XVdxiSLlMURhtVk5Vdj6qllI_p83hWWrAWFCLY6d2VeWRMwSi0ThZhsc5lbMWuiwUE_ICW7b_T4saiVH7hNuLdxpcxq8y6zU_DQYQ-HJe-4iANnLb4jJLdCwEkw_UvRDWP7a3llkg-rMzbQf3orQOpTKEbJBJEl4BMAqrbm8ESj7wh4Gmsc5gc4JbHk4xg8S4B8EYvbkSICQRDe5kR8RMYER9uJJ_s33wrmTamSTVTVA_GJnP0PdhY2LmB4IJ4YbN4gYJDRNpMxRAERgg6ljuaMFe1-BGSRLRozIgRYtSC1VUVBIo6G2Ieub_Z2WtFwFr0ztHlwivjmO2hYX0D8jvMnkiaxL2AN6TePGaNX80x7UPohsRZtKZryr0xZdZBCveSNlM0EKdnuT93KuSKQ9mLS5YFYGXzwerOmvjJybzrZsVWo7CPlZlDonr6Lopj11JqFPmI398bMSRN7N5TSHwBTPti5ufB4Gxzo5MMdF22CGbFCEuQYqio_gX3mt7ATLPUY_Zd1pmfYoXdqzb__qaTDydxYciE-4MI7VpL-KEvO9N8SJ9l2uVM9IN7qjFDSwCUbCxFhcB36uyJtDRltJmIwT_pK6W82YRPCCIUCBRRp84v_VBeJYFxDjmgIdUzA8PGgq0bCoiBZiHlQdogWm2aKhPa8fmxAZa9t4SlyD6eoShJIVQFq_zuSQ2ezF7Ly_dRA9RK5CFWLa8OfPRZCZ6U1Vd02MBWCYvjT9jV5j5OmqWSMLCW217Xpo0UqsEyMMqVywZTRv1awFtslmgxKaZXzUnD754YH.dDljUh-30Qvkt3f7uC0Om3wv6F4XI0M4EPZUWdSiZTs"

# AUTHORIZATION CODE MUST BE PROVIDED AS A VALUE TO THE "code" PARAMETER IN THE PAYLOAD.
payload="grant_type=authorization_code&code=$authorization_code"
payloadDigest=`echo -n "$payload" | openssl dgst -binary -sha256 | openssl base64`
digest=SHA-256=$payloadDigest

reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")

httpHost="https://api.sandbox.ing.com"

# httpMethod value must be in lower case
httpMethod="post"
reqPath="/oauth2/token"

# signingString must be declared exactly as shown below in separate lines
signingString="(request-target): $httpMethod $reqPath
date: $reqDate
digest: $digest"

signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_eidas_client_signing.key" | openssl base64 -A`

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X POST "${httpHost}${reqPath}" \
-H "Accept: application/json" \
-H "Content-Type: application/x-www-form-urlencoded" \
-H "Digest: ${digest}" \
-H "Date: ${reqDate}" \
-H "Authorization: Bearer ${accessToken}" \
-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
-d "${payload}" \
--cert "${certPath}example_eidas_client_tls.cer" \
--key "${certPath}example_eidas_client_tls.key"