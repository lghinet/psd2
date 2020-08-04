#! /usr/bin/env sh

###################################################################################
#                             CALL AIS BALANCE  					              #
###################################################################################
# This script calls the AIS API in sandbox and performs a get request to the-     #
# endpoint "v3/{account-id}/balances" for account balance. You must request       #
# customer access token to call this script.   									  #
# Please update the variables "accessToken" and "certPath".   					  #
###################################################################################

keyId="5ca1ab1e-c0ca-c01a-cafe-154deadbea75" # client_id as provided in the documentation
certPath="/d/GitHub/psd2/ing_psd2/sandbox/ing/" # path of the downloaded certificates and keys
httpHost="https://api.sandbox.ing.com"

# httpMethod value must be in lower case
httpMethod="get"
reqPath="/v3/accounts/a217d676-7559-4f2a-83dc-5da0c2279223/balances?balanceTypes=interimBooked"

# Digest value for an empty body
digest="SHA-256=47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU="

# Generated value of the customer access token. Please note that the customer access token expires in 15 minutes
caccessToken="eyJhbGciOiJkaXIiLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwia2lkIjoidHN0LTRjOGI1Mzk3LTFhYjgtNDFhOC1hNTViLWE3MTk5MTJiODNkMiIsImN0eSI6IkpXVCJ9..c31Sm5fq8KA5C5z94sGOtw.HSXNkTqk8qhXoGOZBD3GPNqOPGo9moSmfWnJlT4Xxb12g3iENuavPz5wecNiNRODnsAl867sPdvgGmCAlKUIiBPlgXHr-zGBk_5Ayz72oz5tHrLCCc9s0KZukbnd4g2TcR4u3rJ_LN5N2FOCvJ4H8bthE3dXwfOXvTF-F4JRMCsXM-VYAiBH3SYNTPodwyX9u9_Zmcln92H4_HiUF0H1-Ho7Bs8fzc1TrauWSPjLJCjGxf8oj8fo-3dUSiF1FoJHE_otBX1gM3BpI3jRrz81UaCzJ3NSmBpcf3jzHsQ8g5QVgcR9cpY8kgKOPJ8SNwriZFTQO4kakAmmi_X8S1-9Vk232SipIM9yr3NHvhfhlTy3uSiKcBQBSlz6WudI6Sb9MUM_rZkLxvZ-J-C068c680hzZrxb4cYa4uMkqJIlShhzEZF0brq52Z6DwrLNlhe18DYomZJROTL-mNFnxEMfGN64F6UXi1ai4EDGjttzBsLPPlttaj0D17aAod-sLBejzIyYlogtuif4iqDWrs2pNBOfXL2lQf619HedvOjp_1M0sS8DSiRvvm2_PQle_IZWnzeRXe1jVEMBO5RCu4HRQxKtRtlibKx38bT7Pgj-XthrAijihwYbVfXm5qN6chv6eaJuP-MTz_FmSSU8ExJ2CYem0gJwv2-rCEwgQ_bP7WYLgLa3Xrp5mwPRZDs7S6g50a6jypmTj6XbKej9YHZ2uIT6SIT-9C9PC67ExrFYBACjA3wbcV3VzeKlIGGJm-9NrXc5NWbvdfGmQyKYx-3Xee67mn1qcI6jDrzgXUoYhwMy81gJcNcABtZuLFkVgGI32IV0bKvBxXU4aDAnd6jmvgxXHP5b-SO1CS6sb5YlZnd-sgkjtBmQ8J_vvWNEi3eCJ0hSgij0IRrC2vFoBMBo8L4tw_LYbMcmIq1TMkOHR7OGCsAkFEi7vkSpQp1ItGLkO_9-6AIZt4KQecQ9GtgRYyjasZWPgQh60xnawZ-TovMtqYH9ntrmMvcWrxKORMi4G8jjAZUoAx4CbyjQqniaFw5MU4LnNOAzeiuiTeqLv6gLRaGzAxUG2E6BwTy7DonUznJ0GWl_L9zeiTq2XlhfOkEX8kMJ4loRFkZhhVaxSEVSgV_weEBwAzUMn8mJ_9Cuki_eWO6Unu8uwE2f4T5NcILGm03W2YOFHbC7fH7zcsw_LeV4FNf7CEhp1AdmJ6JfUm4eXcWUiBkCj5nVVngAdi4xKRXFqIFUtrA3Oqum3YPXQNwB_7_SvnSkcyWSRG9V-Vs7pLS5BCSeMrWZA6R0W567oz-FvWgrfuT6AiKPetMUNftDjuNspSlumoKvRY8cIWKK3NrgZDF0efFgmnhwapBhqAe5Q5Z_MT0KcEns8Bmyo7swqQHGbJVs0hrWOl4wAaGYEEKjI2KDPI7cSEPGvEVnzbq0tMNmSPSynFiLcnEfVY4tE0jmuQOJkjFKZd8Ku6TCB3Ntx_u0CLyaluFPEmgeojZgwgucMxJljnO_Dd1ph5DnNQAB3QemdN3p5mMwq7F9NqtAmLYjdNCl4Z9y3IPjz1dntqq83MzyBO9WFO_O35Irrz9ZHC8xwL5mIv80uEkY3LjZ4ICGfLv3T1rh0vlnFuyxgHlDNxv1uU_9oPicMTfuY7VWwSwOu5U_w8vs0pQ9OHUjXJZiRlRjJnPL4gqhCZ5uUD6cQ_MSfgct8ek6WQtOdWTURuMfX1tdytuPQDUw-9hnbMQILgSiaL-CiWbYp4yTSDDbJer8-NV-DN0ZQ3WhiOF8MUdt1wFBiS-Q08p7ew6-VYpCgwmWfeE3S-ZUSrljj3H4Fq67aV0xkXsN-cPvzxn_Hs-LpPeRb_0EkS18HMV_G94cUEXPFpxUB1DHybzbQoaq0UVmc1M9X-EPxNju3nsp6VOnuiPIHKN1DQW1BrNeD9hAnTxoWEq2FxFGk1b95ijM5bIm7KmYL6BXTAosS96TGO3kaaGwnECzz6izd-j8EtDB-CfFKromKVKV1Rg8odm6JUmYwGMp7ged43ovyiypIpRt9InnVtrmIUd2-E0LhtT1LBMNdK8Pk7n6uSBCEn2Q5egYCkKZks70YF-qHjBH_KgqTxbSXqdzS08La2WW4FKwFW_J2p-7fhS8t3xHMOYN8fuFU1GODP9wlEDxiiqFFTSD9igCxpAGQdx6xNCspydVW9PnEQ8yy1i07Bu9eAdnzf0dwd8Z74oZQibeW3t6UKjN4a0cYvfc0EBhiAtnEFdbssvcMGLxWdEQFg2QjRpiWWuJNsjOhox87IWOqn-NAUnUBA35MNr-Bh6jH_vQpCUNpO2t2bCHyQ8NDhu2Dtz_TCOufXk5d-YnYj-EjAHdOboD8b23Tu8eg0C5GmKta31eU-v8YJ6rAUua6J6arPvUAPNPVlLKX_uy89dY9T8gUmPiitT5W3aAxe9pBZZ6cwlKqGQ0woYjxL8hzhp0yi1FHsRt18U2YRdJMBfuGAWnkHWqaltxJJOUtImDQYYeYRJNMUYQP9MbegKKczbyTT_aU2dDqzqW5k4oRTSWMQm7BrNNt5er_gunw3HIlzWXp9XVAJjdQx8fM-G0aTLIsqR8izBuFVC8osxiteSCpkoFKAtMHterRG5phj-9qU08KlZyO7VY8UtVionExPG31d4q3W7A-tIJAJNEjafhLAUt67nt8hQQ76h2urzydHne_xNwIrWM656IMfmkkSQ42GdF-eZjH4Qk3JUkiKo46_fVtbrUHoT1S07-6-gZ6PaL_2bGwZCLQVUPmnMg6EUzAZTz8glAYiQOcjfZF0516ahpQkewdbVuXEqRRfZTqj5NoImLwiNxcc2ei3UiFSNdalIgdPuWvYIxRIsLV2C_AaWxBk1yStEb0Xp2EYJAd3u-0t2XbwNZFSETosr0a6IJnmmt8ld35oxzD3s.wR6VAVlOM-0yMGdVqOm459JhT0VQGvOrYXnfTYjcXHk"

reqDate=$(LC_TIME=en_US.UTF-8 date -u "+%a, %d %b %Y %H:%M:%S GMT")

# signingString must be declared exactly as shown below in separate lines
signingString="(request-target): $httpMethod $reqPath
date: $reqDate
digest: $digest"

# signingString must be declared exactly as shown below in separate lines
signature=`printf %s "$signingString" | openssl dgst -sha256 -sign "${certPath}example_eidas_client_signing.key" | openssl base64 -A`

# Curl request method must be in uppercase e.g "POST", "GET"
curl -i -X GET "${httpHost}$reqPath" \
-H "Accept: application/json" \
-H "Content-Type: application/json" \
-H "Digest: ${digest}" \
-H "Date: ${reqDate}" \
-H "Authorization: Bearer ${caccessToken}" \
-H "Signature: keyId=\"$keyId\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"$signature\"" \
--cert "${certPath}example_eidas_client_tls.cer" \
--key "${certPath}example_eidas_client_tls.key"