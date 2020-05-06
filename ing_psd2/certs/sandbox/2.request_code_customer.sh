#! /usr/bin/env sh

###################################################################################
#                             REQUEST URL TO ING AUTHORIZATION APP                #
###################################################################################
# This script calls the endpoint "oauth2/authorization-server-url" to request     #
# an authorization code for requesting customer access token. In this script      #
# we pass "payment-account:balance:view" and "payment-accounts:transactions:view" #
# scope tokens to consume AIS API. You must request an application access token   #
# to run this script. Please update the variables "accessToken" and "certPath".   #
###################################################################################


keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
certPath="/d/GitHub/psd2/ing_psd2/certs/sandbox/" # path of the downloaded certificates and keys
httpHost="https://api.sandbox.ing.com"

# httpMethod must be in lower code
httpMethod="get"
reqPath="/oauth2/authorization-server-url?scope=payment-accounts%3Abalances%3Aview%20payment-accounts%3Atransactions%3Aview&redirect_uri=https://www.example.com&country_code=NL"

# Digest value for an empty body
digest="SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="


# Generated value of the application access token. Please note that the access token expires in 15 minutes
accessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..90qcJuubDGtlBoreLziGHw.eIhHoTYaycaevnGeyz4Ny3mCnOy1TKKTPMpImkt9a3MEAWrSknAr-l5kM5E5NFpHODb_XVHn2J5w5J4QY3gh2HyJ5oGfXp_BOmlwiBNtLooZK9dnynMRVJI_VR_a1TXg_kU2rsFHL0bK1Y4t6JNQjUvpqk55Cwv_pmPMSOY9vOBpY9vTN5TnDFysXpWdGO2ULLnJojUjGO0CBhlrJL8WnJVJv1Bs2vyAFxG5RnY4Z_ObI3OMOPTVTO8_bXUNWl5OeZL5YPY-9e_Od0NZJrw-MsGOALf9Y_x16VGBn5zG30kalpXUvTefQo6V9vtETz089g0C8eidFhnuKXXmenl0Q471PzSN14T64JNCyJCQU1FaSZ9bIA3ecziwbkpcxwltKx_ZVV_xlxB0mxC1175lThhEfu6q_Pxj8I_U3v_Mv8H4L0MZOfkSEjm61dWkZ08gZG19fWbzCFpfpuETiuUGVvCseUARn6Y2Buw0sC8516RdBNPmETr5yWwewgK5g_cchJM1lDF8E-BQvv3YTxulRfeQXv12jY0l2D0GnfqQAv3vicOy84fn6zBUcWLrknMvdPAWeuT-AL6Q56SsglBbfx9W_bDDrynMLrtin-LhEtkbSmsrgu7FUT7oAfBZNc8L6lea-8v5X7eQROsZMjBG63UofM0Kr8ajNvT4LyhXOrn4zj-N_p-dX5T-6RghDmabAcQj81FF0SGZQA9MpHKPWq8mRYZ73dcPhiCcCRK9CMWOsvbzZ6daklyF_MMOb3zBTAMTO9KT87vGUr5RaDCgroGABifKdWpsgkH9SBLO_k8VyQJL0AOAWNrhJZtDwJ_SHIkh-kRCcXO0Z0WbTV0RURI6hi5rz0QCv-kG_QFYx78Ah_LmKrAehpHB_lQBwyACStcJ0YzyGDV9QrO4b6ceKgovILy1KHL3DmxtcNa_soV3z34F_Vljb7hUPsUxnZwp_QAHpF-EuRTn6-7i7Yc7aXa6YNbT00Dmj3xjqXad3_x25ZhNSFp1S1twzQiQ6XRCO9xPzeCraBd8G5F5yjRogfNzzZAhRvRGoHD6q2uDOzwkvJ-X6Hm8L2ZL1r5L0uRYm5ocNVTt6G3egx3s2_ZOdyzw8SOjy9gUQ0d4HzEpPQEyZcH40-zobhjhM-Ptf28dWnHqq8aA0VwtGV6YU8pDlex-cIehJPuPXpJl2ij-8X9Ox9-fBS-kT2Qe7lHM9pdUCK5COQHNusvacQyTPw1ekzYOmf4cyWI6Y7ZstNAiSef22EpN0vsYzRpGHKXx1zHI4R1wNYUZFxOIn_OYCIQJ4UCw_2pCHC-ITknqKg6cfcv3RWrJOyADw4dHf5deYAt6tPS57a0dUq8K5ireVvFDjFHrO89MD6yKzLqBmglBye2XVdGpOfowJ4Wm-mPOuOuAmAUabjP7R17PLTUqaYMbmNndh6shfOe5Rabp-WLV03IzkrDrdOXtrZbQAyS1Yfi_kMl0JIQuw8qqsdZRD5GtSqdSn88rOVgeUIsjwoXwTPu9PgwZFv4Dd4AnZTF8ZVjyWGkW0oNtAwX2TO7h28C9ktTMLExH1no1s1FERxDpWPRgzc4BP6uDRsN9EZkIR5TTdKOmeQdYf6I0dB-QMrwREmpJIHNbrYEyNuHTE5BNBxllLfJeWT7U4VNVJQJkvCC8Uh-muCSMbcRvo8YWtdBkSsHpMT6HO053GB_oP_QxzvYQASMIWULNYFUU5WVFAsdiwiWtrCYvzZb6Nf8jyNacwFwG5LjtcVZT6KhfWrqNKO-uRkUBcQCZzkgydgShsyIuYsReSd-hTRaTO38UlQ333R9JVb6u6OADTUEXhWdqB84yN79Eejps6Zb6JpD91tJcmTp_3r8W3OYMxzWVQz3u8KP8VIziqS3QaNG22Mc_ts3T0fOzEs5kO-aRPISpmd4aLqdCgTkmUCUjkgLZ6IKaStyDU89tlSyZWyhRiR6OqbBfOJJMf33Fo7wwmSDCwoG9lu9l5-71CsA_xbuO_OKJ5QhgStbjfO-AAVePYi7RGILELX_jVaistBpsZHFPd-BWuu8PQy6vcd02zCyZoxug0gJWNiWdE4ckmfnSSs5RWbVv0HbR8mz8HxVaTcpYV_kPAd10EKuBfUWNTcELeLYqv41h3xrPA-JB_v7uYafbRoN_cBwVqwEJNtLW5FwnCIGWPEzdvUTISBb-vb-MJiybE46n3kTAc4PXEIUZNA_umPcwMzEsIOLeny6G1XaI6x9I3XIPGWAu0cYTQ3alr7hHu7SiruHrgC75os6kCXNa_lNxvguBrwdKxIWawwgn6IJe.Lrl-RUGILwlKqBPHV-zZYA3WQEgjE40drRG4Ml3Tjo0"

reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")

# signingString must be declared exactly as shown below in separate lines
signingString="(request-target): $httpMethod $reqPath
date: $reqDate
digest: $digest"

signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_eidas_client_signing.key" | openssl base64 -A`

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X GET "${httpHost}${reqPath}" \
-H "Accept: application/json" \
-H "Content-Type: application/json" \
-H "Digest: ${digest}" \
-H "Date: ${reqDate}" \
-H "Authorization: Bearer ${accessToken}" \
-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
-d "${payload}" \
--cert "${certPath}example_eidas_client_tls.cer" \
--key "${certPath}example_eidas_client_tls.key"