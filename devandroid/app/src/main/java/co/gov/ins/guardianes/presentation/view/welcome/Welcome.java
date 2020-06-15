package co.gov.ins.guardianes.presentation.view.welcome;

import co.gov.ins.guardianes.R;

/**
 * @author Igor Morais
 */
public enum Welcome {

    TUTO1     (1, R.layout.tutorial1),
    TUTO2     (2, R.layout.tutorial2),
    TUTO3     (3, R.layout.tutorial3),
    TUTO4     (4, R.layout.tutorial4);

    private final int id;
    private final int layout;

    Welcome(final int id, final int layout) {
        this.id = id;
        this.layout = layout;
    }

    public final int getId() {
        return id;
    }

    public final int getLayout() {
        return layout;
    }

    public static Welcome getBy(final int id) {

        for (final Welcome welcome : Welcome.values()) {

            if (welcome.getId() == id) {
                return welcome;
            }
        }

        throw new IllegalArgumentException("The Welcome has not found.");
    }
}
