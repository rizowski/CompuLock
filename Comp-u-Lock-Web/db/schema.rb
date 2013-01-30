# encoding: UTF-8
# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended to check this file into your version control system.

ActiveRecord::Schema.define(:version => 20130130013059) do

  create_table "account_histories", :force => true do |t|
    t.integer  "account_id"
    t.string   "domain"
    t.string   "url"
    t.string   "title"
    t.datetime "last_visited"
    t.integer  "visit_count"
    t.datetime "created_at",   :null => false
    t.datetime "updated_at",   :null => false
  end

  create_table "account_processes", :force => true do |t|
    t.integer  "account_id"
    t.string   "name"
    t.datetime "lastrun"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "account_programs", :force => true do |t|
    t.integer  "account_id"
    t.string   "name"
    t.datetime "lastrun"
    t.integer  "open_count"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "accounts", :force => true do |t|
    t.integer  "computer_id"
    t.string   "domain"
    t.string   "user_name"
    t.boolean  "tracking"
    t.time     "allotted_time"
    t.time     "used_time"
    t.datetime "created_at",    :null => false
    t.datetime "updated_at",    :null => false
  end

  create_table "computers", :force => true do |t|
    t.integer  "user_id"
    t.string   "name"
    t.string   "ip_address"
    t.string   "enviroment"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "users", :force => true do |t|
    t.integer  "computer_id"
    t.string   "username"
    t.string   "email",                  :default => "",    :null => false
    t.string   "encrypted_password",     :default => "",    :null => false
    t.boolean  "admin",                  :default => false
    t.string   "reset_password_token"
    t.datetime "reset_password_sent_at"
    t.datetime "remember_created_at"
    t.integer  "sign_in_count",          :default => 0
    t.datetime "current_sign_in_at"
    t.datetime "last_sign_in_at"
    t.string   "current_sign_in_ip"
    t.string   "last_sign_in_ip"
    t.datetime "created_at",                                :null => false
    t.datetime "updated_at",                                :null => false
  end

  add_index "users", ["email"], :name => "index_users_on_email", :unique => true
  add_index "users", ["reset_password_token"], :name => "index_users_on_reset_password_token", :unique => true

end
