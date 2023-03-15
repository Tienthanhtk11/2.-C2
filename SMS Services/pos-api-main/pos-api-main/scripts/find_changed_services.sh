#!/bin/bash

list_services=$(find -maxdepth 2 -type d | grep -E 'Point_of_Sale$' | awk -F/ '{print $3}')
list_changed_services=()

for service in ${list_services[@]}; do
    list_changed_services+="$(git diff HEAD~ --name-only | grep -Po ${service} | uniq) "
done

echo $list_changed_services
