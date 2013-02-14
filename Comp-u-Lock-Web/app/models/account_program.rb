class AccountProgram < ActiveRecord::Base
  	attr_accessible :account_id, :lastrun, :name, :open_count
  
  	validates :name, presence: true
  	validates :lastrun, presence: true
  	validates :account_id, presence: true

	belongs_to :account
end
