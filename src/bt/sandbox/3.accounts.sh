#!/bin/bash

curl --request GET \
  --url 'https://api.apistorebt.ro/bt/sb/bt-psd2-aisp/v1/accounts?withBalance=REPLACE_THIS_VALUE' \
  --header 'accept: application/json' \
  --header 'authorization: Bearer a7iyAnd9EItRweJV6CAY' \
  --header 'consent-id: 273c540e6c534b1f8d873baf23728969' \
  --header 'psu-geo-location: 46.931808,26.369690' \
  --header 'psu-ip-address: 128.0.57.43' \
  --header 'x-request-id: c177448d-ded3-4f3a-8950-4838ab636f33'