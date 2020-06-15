package com.epm.app.dto;

/**
 * Created by leidycarolinazuluagabastidas on 30/03/17.
 */

public class NoticiasEventosDTO {

    private String IdNews;
    private String Title;
    private String Date;
    private String NewsText;
    private String Summary;
    private String UrlImage;

    public String getIdNews() {
        return IdNews;
    }

    public void setIdNews(String idNews) {
        IdNews = idNews;
    }

    public String getTitle() {
        return Title;
    }

    public void setTitle(String title) {
        Title = title;
    }

    public String getDate() {
        return Date;
    }

    public void setDate(String date) {
        Date = date;
    }

    public String getNewsText() {
        return NewsText;
    }

    public void setNewsText(String newsText) {
        NewsText = newsText;
    }

    public String getSummary() {
        return Summary;
    }

    public void setSummary(String summary) {
        Summary = summary;
    }

    public String getUrlImage() {
        return UrlImage;
    }

    public void setUrlImage(String urlImage) {
        UrlImage = urlImage;
    }
}
