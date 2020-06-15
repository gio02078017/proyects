package com.epm.app.business_models.dto;

/**
 * Created by josetabaresramirez on 24/02/17.
 */

public class AddressFromArcgisDTO {


    private Address address;

    public Address getAddress() {
        return address;
    }

    public void setAddress(Address address) {
        this.address = address;
    }

    public static class Address {

        private String Match_addr;
        private String City;
        private String Region;
        private String CountryCode;

        public String getMatch_addr() {
            return Match_addr;
        }

        public void setMatch_addr(String match_addr) {
            Match_addr = match_addr;
        }

        public String getCity() {
            return City;
        }

        public void setCity(String city) {
            City = city;
        }

        public String getRegion() {
            return Region;
        }

        public void setRegion(String region) {
            Region = region;
        }

        public String getCountryCode() {
            return CountryCode;
        }

        public void setCountryCode(String countryCode) {
            CountryCode = countryCode;
        }

    }
}
