FROM mariadb:11.2

ENV MYSQL_ROOT_PASSWORD=rootpassword
ENV MYSQL_DATABASE=smart_traffic
ENV MYSQL_USER=traffic_user
ENV MYSQL_PASSWORD=traffic_password

EXPOSE 3306

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD ["mysqladmin", "ping", "-h", "localhost", "-u", "root", "-p${MYSQL_ROOT_PASSWORD}"]